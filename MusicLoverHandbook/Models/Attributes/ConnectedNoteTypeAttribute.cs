﻿namespace MusicLoverHandbook.Models.Attributes
{
    public class AssociatedTypeAttribute : Attribute
    {
        #region Public Properties

        public Type Type { get; }

        #endregion Public Properties

        #region Public Constructors + Destructors

        public AssociatedTypeAttribute(Type noteType)
        {
            Type = noteType;
        }

        #endregion Public Constructors + Destructors
    }
}
