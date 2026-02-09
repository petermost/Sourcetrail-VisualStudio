/*
 * Copyright 2018 Coati Software KG
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using CoatiSoftware.SourcetrailExtension.Logging;
using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace VCProjectEngineWrapper
{
	public class VCConfigurationWrapperVs2026 : IVCConfigurationWrapper
	{
		private VCConfiguration _wrapped = null;

		public VCConfigurationWrapperVs2026(object wrapped) // TODO: Use concrete type
		{
			_wrapped = wrapped as VCConfiguration;
		}

		public bool isValid()
		{
			return (_wrapped != null);
		}

		public string GetWrappedVersion()
		{
			return Utility.GetWrappedVersion();
		}

		public bool isMakefileConfiguration()
		{
			try
			{
				ConfigurationTypes configurationType = _wrapped.ConfigurationType;
				if (configurationType == ConfigurationTypes.typeGeneric)
				{
					return true;
				}

				if (configurationType == ConfigurationTypes.typeUnknown && GetNMakeTool() != null && GetNMakeTool().isValid())
				{
					return true;
				}
			}
			catch
			{
				Logging.LogWarning("Unable to determine if a makefile configuration is used, falling back to default behavior.");
			}

			return false;
		}

		public string EvaluateMacro(string macro)
		{
			return _wrapped.Evaluate(macro);
		}

		public IVCCLCompilerToolWrapper GetCLCompilerTool()
		{
			try
			{
				IEnumerable tools = _wrapped.Tools as IEnumerable;
				foreach (Object tool in tools) // TODO: Use concrete type
				{
					VCCLCompilerTool compilerTool = tool as VCCLCompilerTool;
					if (compilerTool != null)
					{
						return new VCCLCompilerToolWrapperVs2026(compilerTool);
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Configuration failed to retreive cl compiler tool: " + e.Message);
			}
			return new VCCLCompilerToolWrapperVs2026(null);
		}

		public IVCNMakeToolWrapper GetNMakeTool()
		{
			try
			{
				IEnumerable tools = _wrapped.Tools as IEnumerable;

				foreach (Object tool in tools)
				{
					VCNMakeTool compilerTool = tool as VCNMakeTool;
					if (compilerTool != null)
					{
						return new VCNMakeToolWrapperVs2026(compilerTool);
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Configuration failed to retreive nmake tool: " + e.Message);
			}
			return new VCNMakeToolWrapperVs2026(null);
		}

		public List<IVCPropertySheetWrapper> GetPropertySheets()
		{
			List<IVCPropertySheetWrapper> propertySheetsWrappers = new List<IVCPropertySheetWrapper>();
			try
			{
				IEnumerable wrappedPropertySheets = _wrapped.PropertySheets;
				foreach (Object wrappedPropertySheet in wrappedPropertySheets)
				{
					VCPropertySheet vcPropertySheet = wrappedPropertySheet as VCPropertySheet;
					if (vcPropertySheet != null)
					{
						IVCPropertySheetWrapper wrapper = new VCPropertySheetWrapperVs2026(vcPropertySheet);

						if (wrapper != null && wrapper.isValid())
						{
							propertySheetsWrappers.Add(wrapper);
						}
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Configuration failed to retreive property sheets: " + e.Message);
			}

			return propertySheetsWrappers;
		}

		public IVCPlatformWrapper GetPlatform()
		{
			return new VCPlatformWrapperVs2026(_wrapped.Platform);
		}
	}
}

