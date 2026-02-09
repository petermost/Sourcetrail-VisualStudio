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

using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections.Generic;

namespace VCProjectEngineWrapper
{
	public class VCFileWrapperVs2026 : IVCFileWrapper
	{
		private VCFile _wrapped = null;

		public VCFileWrapperVs2026(object wrapped) // TODO: Use concrete type
		{
			_wrapped = wrapped as VCFile;
		}

		public bool isValid()
		{
			return (_wrapped != null);
		}

		public string GetWrappedVersion()
		{
			return Utility.GetWrappedVersion();
		}

		public string GetSubType()
		{
			return _wrapped.SubType;
		}

		public IVCProjectWrapper GetProject()
		{
			return new VCProjectWrapperVs2026(_wrapped.project);
		}

		public List<IVCFileConfigurationWrapper> GetFileConfigurations()
		{
			List<IVCFileConfigurationWrapper> fileConfigurations = new List<IVCFileConfigurationWrapper>();
			foreach (Object configuration in _wrapped.FileConfigurations)
			{
				IVCFileConfigurationWrapper vcFileConfig = new VCFileConfigurationWrapperVs2026(configuration);

				if (vcFileConfig.isValid())
				{
					fileConfigurations.Add(vcFileConfig);
				}
			}
			return fileConfigurations;
		}
	}
}
