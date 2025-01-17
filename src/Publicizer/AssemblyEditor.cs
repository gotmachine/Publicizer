﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Publicizer
{
    /// <summary>
    /// Class for making edits to assemblies and related types.
    /// </summary>
    public static class AssemblyEditor
    {
        public static void PublicizeType(TypeDef type)
        {
            type.Attributes &= ~TypeAttributes.VisibilityMask;

            if (type.IsNested)
            {
                type.Attributes |= TypeAttributes.NestedPublic;
            }
            else
            {
                type.Attributes |= TypeAttributes.Public;
            }
        }

        public static void PublicizeProperty(PropertyDef property, bool publicizeAsReferenceAssemblies)
        {
            if (property.GetMethod is MethodDef getMethod)
            {
                PublicizeMethod(getMethod, publicizeAsReferenceAssemblies);
            }

            if (property.SetMethod is MethodDef setMethod)
            {
                PublicizeMethod(setMethod, publicizeAsReferenceAssemblies);
            }
        }

        public static void PublicizeMethod(MethodDef method, bool publicizeAsReferenceAssemblies)
        {
            method.Attributes &= ~MethodAttributes.MemberAccessMask;
            method.Attributes |= MethodAttributes.Public;

            if (publicizeAsReferenceAssemblies)
            {
                StripMethodBody(method);
            }
        }

        public static void PublicizeField(FieldDef field)
        {
            field.Attributes &= ~FieldAttributes.FieldAccessMask;
            field.Attributes |= FieldAttributes.Public;
        }

        public static void StripMethodBody(MethodDef method)
        {
            method.Body = new CilBody();
            method.Body.Instructions.Add(new Instruction(OpCodes.Ret));
        }
    }
}
