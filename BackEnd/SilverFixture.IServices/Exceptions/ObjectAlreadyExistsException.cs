﻿using System;

namespace SilverFixture.IServices.Exceptions
{
    public class ObjectAlreadyExistsException : Exception
    {
        public object AlreadyExistingObject { get; private set; }

        public ObjectAlreadyExistsException(object alreadyExistingObject)
        {
            AlreadyExistingObject = alreadyExistingObject;
        }

        public override string Message => $"Object {AlreadyExistingObject} already exists in the system";
    }
}
