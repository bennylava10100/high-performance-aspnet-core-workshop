﻿using System;

namespace GettingThingsDone.Contracts.Model
{
    /// <summary>
    /// Represents a GTD action.
    /// </summary>
    public class Action : Entity
    {
        /// <summary>
        /// Action title. Title is mandatory.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Optional due date.
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// Optional date on which the action is planned to be done.
        /// It must be smaller then <see cref="DueDate"/>.
        /// </summary>
        public DateTimeOffset? DoOn { get; set; }

        /// <summary>
        /// The date on which action is done, or null if the action is still undone.
        /// </summary>
        public DateTimeOffset? DoneAt { get; set; }

        /// <summary>
        /// True if the action is done. A done action cannot be changed.
        /// </summary>
        public bool IsDone => DoneAt != null;

        /// <summary>
        /// The ID of the list to which this action belongs or null if it doesn't belong to any list.
        /// </summary>
        public int? ListId { get; set; }

        /// <summary>
        /// The list to which this action belongs or null if it doesn't belong to any list.
        /// </summary>
        public ActionList List { get; set; }

        /// <summary>
        /// The ID of the project to which this action belongs or null if it doesn't belong to any project.
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// The project to which this action belongs or null if it doesn't belong to any project.
        /// </summary>
        public Project Project { get; set; }
    }
}