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

namespace VCProjectEngineWrapper
{
	public class VCPropertySheetWrapperVs2026 : IVCPropertySheetWrapper
	{
		private VCPropertySheet _wrapped = null;

		public VCPropertySheetWrapperVs2026(object wrapped) // TODO: Use concrete type
		{
			_wrapped = wrapped as VCPropertySheet;
		}

		public bool isValid()
		{
			return (_wrapped != null);
		}

		public string GetWrappedVersion()
		{
			return Utility.GetWrappedVersion();
		}

		public string getName()
		{
			return _wrapped.Name;
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
				Logging.LogError("Property Sheet failed to retreive cl compiler tool: " + e.Message);
			}
			return new VCCLCompilerToolWrapperVs2026(null);
		}

		public IVCResourceCompilerToolWrapper GetResourceCompilerTool()
		{
			try
			{
				IEnumerable tools = _wrapped.Tools as IEnumerable;
				foreach (Object tool in tools) // TODO: Use concrete type
				{
					VCResourceCompilerTool compilerTool = tool as VCResourceCompilerTool;
					if (compilerTool != null)
					{
						return new VCResourceCompilerToolWrapperVs2026(compilerTool);
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Property Sheet failed to retreive resource compiler tool: " + e.Message);
			}
			return new VCResourceCompilerToolWrapperVs2026(null);
		}

	}
}
