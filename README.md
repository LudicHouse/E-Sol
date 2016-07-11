# E-Sol App
The E-Sol App is part of the MyGEOSS project on innovative apps in environmental and social domains 

The app is published under the European Union Public License (EUPL) Version 1.1. See LICENSE file.

## App Stores
The app is currently not published in the Apple App Store nor in the Google Play Store. 
The APK package for Google Android is available in this repository within the "Client Executable" folder.

# Project Workflow
This project currently uses the [Git Flow](http://nvie.com/posts/a-successful-git-branching-model/) system of branch management. If you're unfamiliar system, the only bits that regular developers/contributors need to know is that the 'master' branch represents the live, released version of the game, while 'develop' is the in-progress branch containing all completed features/changes. If anyone wants to add a feature or make a change, just create a new branch off develop and work on it there, then once finished use GitHub's Pull Request feature to mark it as ready to merge back into develop (the site may try and request a merge into master by default, you may need to change that manually when creating the pull request).

## Merge Conflicts
The Unity project has been set up to simplify merging branches as much as possible (the Visible Assets mode is enabled to manage asset tracking, and all Unity-generated binary files have been converted to a plaintext YAML format), though if problematic merge conflics occur that cannot be resolved using simple text editing Unity provides a [merge tool](https://docs.unity3d.com/Manual/SmartMerge.html) to simplify the process. 

# Funding
This application has been developed within the MyGEOSS project, which has received funding from the European Union’s Horizon 2020 research and innovation programme.

# Disclaimer
The JRC, or as the case may be the European Commission, shall not be held liable for any direct or indirect, incidental, consequential or other damages, including but not limited to the loss of data, loss of profits, or any other financial loss arising from the use of this application, or inability to use it, even if the JRC is notified of the possibility of such damages.

# License
Copyright 2016 EUROPEAN UNION Licensed under the EUPL, Version 1.1 or - as soon they will be approved by the European Commission - subsequent versions of the EUPL (the "Licence"); You may not use this work except in compliance with the Licence. A copy of the License is included in this repository (see LICENSE file), you may obtain a copy of the Licence at: http://ec.europa.eu/idabc/eupl Unless required by applicable law or agreed to in writing, software distributed under the Licence is distributed on an "AS IS" basis, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the Licence for the specific language governing permissions and limitations under the Licence.

* Originally published by the European Commission at https://webgate.ec.europa.eu/CITnet/stash/projects/MYGEOSS/repos/e-sol/browse on 5/11/2015.
* Modified by the Ludic House development team starting 7/7/2016.
