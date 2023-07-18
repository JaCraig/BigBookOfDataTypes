/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

namespace BigBook.EventArgs
{
    /// <summary>
    /// Base event args for the events used in the system
    /// </summary>
    public class BaseEventArgs : System.EventArgs
    {
        /// <summary>
        /// Content of the event
        /// </summary>
        public object? Content { get; set; }

        /// <summary>
        /// Should the event be stopped?
        /// </summary>
        public bool Stop { get; set; }
    }

    /// <summary>
    /// Changed event args
    /// </summary>
    public class ChangedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Deleted event args
    /// </summary>
    public class DeletedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Deleting event args
    /// </summary>
    public class DeletingEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Loaded event args
    /// </summary>
    public class LoadedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Loading event args
    /// </summary>
    public class LoadingEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// On end event args
    /// </summary>
    public class OnEndEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// On error event args
    /// </summary>
    public class OnErrorEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// On start event args
    /// </summary>
    public class OnStartEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Saved event args
    /// </summary>
    public class SavedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Saving event args
    /// </summary>
    public class SavingEventArgs : BaseEventArgs
    {
    }
}