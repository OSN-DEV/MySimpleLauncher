using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySimpleLauncher.Util {
    internal static class Wording {
        #region Declaration
        internal static string NoProfile = "No Profile";
        #endregion
    }

    internal static class ErrorMsg {
        internal static string ProfileNotFound = "Profile [{0}] not found";
        internal static string ProfileIsRegistered = "selected profile is already exist";
        internal static string FailToInsert = "fail to insert {0}.";
        internal static string FailToDelete = "fail to delete {0}.";
        internal static string FailToUpdate = "fail to update {0}.";
        internal static string FailToCreate = "fail to create {0}.";
        internal static string UnExpectedError = "UnExpected error occurred.";
        internal static string FailToLaunch = "fail to launch {0}.";
    }

    internal static class ConfirmMsg {

    }

}
