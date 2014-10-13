using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using Microsoft.CSharp;
using System.IO;
using System.CodeDom.Compiler;

namespace BISTel.eSPC.Common
{
    public class DynamicCodeGenerator
    {
        public enum PropertyType
        {
            GET,
            SET,
            BOTH
        }

        CodeCompileUnit unit;
        CodeNamespace customEntityRoot;
        CodeTypeDeclaration derived;

        string _code;
        string _nameSpace;
        string _className;
        string _baseType;

        #region Class

        public DynamicCodeGenerator(string nameSpace, string className)
        {
            this.InitializeClass(nameSpace, className, null);
        }

        public DynamicCodeGenerator(string nameSpace, string className, string baseType)
        {
            this.InitializeClass(nameSpace, className, baseType);
        }

        private void InitializeClass(string nameSpace, string className, string baseType)
        {
            _nameSpace = nameSpace;
            _className = className;
            _baseType = baseType;

            unit = new CodeCompileUnit();

            //Create a namespace
            customEntityRoot = new CodeNamespace(nameSpace);
            unit.Namespaces.Add(customEntityRoot);

            //Create class
            derived = new CodeTypeDeclaration(className);

            customEntityRoot.Types.Add(derived);//Add the class to namespace defined above

            //Create constructor
            CodeConstructor derivedClassConstructor = new CodeConstructor();
            derivedClassConstructor.Attributes = MemberAttributes.Public;

            derived.Members.Add(derivedClassConstructor);//Add constructor to class

            if (baseType != null && baseType.Length > 0)
            {
                derived.BaseTypes.Add(new CodeTypeReference(baseType));
            }



        }

        #endregion


        public void AddImports(string importName)
        {
            //customEntityRoot.Imports.Add(new CodeNamespaceImport("System"));//Add references
            customEntityRoot.Imports.Add(new CodeNamespaceImport(importName));
        }

        public void AddReference(string referenceName)
        {
            //unit.ReferencedAssemblies.Add("System.dll");
            unit.ReferencedAssemblies.Add(referenceName);
        }


        public void AddClassAttribute(string attribueTypeName, string argumentName)
        {
            this.AddClassAttribute(attribueTypeName, new CodePrimitiveExpression(argumentName));
        }

        public void AddClassAttribute(string attribueTypeName, CodeExpression argumentExpression)
        {
            CodeAttributeArgument argument = new CodeAttributeArgument(argumentExpression);
            CodeAttributeDeclaration classLevelAttribute = new CodeAttributeDeclaration(new CodeTypeReference(attribueTypeName), argument);//Create attribute to be added to class

            derived.CustomAttributes.Add(classLevelAttribute);
        }

        #region Member

        public void AddMember(CodeMemberField codeMeberField)
        {
            derived.Members.Add(codeMeberField);
        }

        public CodeMemberField AddMember(MemberAttributes attribute, string memberName, Type memberType, CodeExpression initailValue)
        {
            return this.AddMember(attribute, memberName, memberType.Name, initailValue);
        }

        public CodeMemberField AddMember(MemberAttributes attribute, string memberName, string memberTypeName, CodeExpression initailValue)
        {
            CodeMemberField clsMember = new CodeMemberField();
            clsMember.Name = memberName;
            clsMember.Attributes = attribute;
            clsMember.Type = new CodeTypeReference(memberTypeName);

            if (initailValue != null)
                clsMember.InitExpression = initailValue;

            derived.Members.Add(clsMember);
            return clsMember;
        }


        #endregion


        #region Method

        public void AddMethod(CodeMemberMethod codeMemberMethod)
        {
            derived.Members.Add(codeMemberMethod);
        }

        public CodeMemberMethod AddMethod(MemberAttributes attribute, string methodName, Type returnType, string methodBody, params CodeParameterDeclarationExpression[] codeParams)
        {
            return this.AddMethod(attribute, methodName, returnType.Name, methodBody, codeParams);
        }

        public CodeMemberMethod AddMethod(MemberAttributes attribute, string methodName, string returnTypeName, string methodBody, params CodeParameterDeclarationExpression[] codeParams)
        {
            CodeMemberMethod derivedMethod = new CodeMemberMethod();
            //derivedMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override; //Make this method an override of base class's method
            derivedMethod.Attributes = attribute;
            //derivedMethod.Comments.Add(new CodeCommentStatement(new CodeComment("TestComment")));
            derivedMethod.Name = methodName;
            derivedMethod.ReturnType = new CodeTypeReference(returnTypeName);
            //arguments = new CodeAttributeArgument[2];
            //arguments[0] = new CodeAttributeArgument(new CodeSnippetExpression("ComplexityLevel.SuperComplex"));//Create parameter for attribute
            //arguments[1] = new CodeAttributeArgument(new CodePrimitiveExpression(methodAuthorName));
            //CodeAttributeDeclaration methodLevelAttribute = new CodeAttributeDeclaration(new CodeTypeReference("DynamicCodeGeneration.CustomAttributes.MethodLevelAttribute"), arguments);//Create attribute to be added to method
            //derivedMethod.CustomAttributes.Add(methodLevelAttribute);//Add attribute to method

            foreach (CodeParameterDeclarationExpression codeParam in codeParams)
            {
                //CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression("System.String", "stringParam");
                derivedMethod.Parameters.Add(codeParam);
            }

            CodeSnippetStatement code = new CodeSnippetStatement(methodBody);
            derivedMethod.Statements.Add(code);

            derived.Members.Add(derivedMethod);//Add method to the class

            return derivedMethod;

            //// ## Declares a method Sample ##
            //CodeMemberMethod method1 = new CodeMemberMethod();
            //method1.Name = "TestMethod";
            //// Declares a string parameter passed by reference.
            //CodeParameterDeclarationExpression param1 = new CodeParameterDeclarationExpression("System.String", "stringParam");
            //param1.Direction = FieldDirection.Ref;
            //method1.Parameters.Add(param1);
            //// Declares a Int32 parameter passed by incoming field.
            //CodeParameterDeclarationExpression param2 = new CodeParameterDeclarationExpression("System.Int32", "intParam");
            //param2.Direction = FieldDirection.Out;
            //method1.Parameters.Add(param2);
            //// A C# code generator produces the following source code for the preceeding example code:
            ////        private void TestMethod(ref string stringParam, out int intParam) {
            ////        }
        }

        #endregion

        #region Property

        public void AddProperty(CodeMemberProperty codeMemberProperty)
        {
            derived.Members.Add(codeMemberProperty);
        }

        public CodeMemberProperty AddProperty(MemberAttributes attribute, Type propType, string propName, string displayName, PropertyType satementType, string referenceFieldName, string category, string description, bool browsable)
        {
            return this.AddProperty(attribute, propType.Name, propName, displayName, satementType, referenceFieldName, category, description, browsable);
        }

        public CodeMemberProperty AddProperty(MemberAttributes attribute, string propTypeName, string propName, string displayName, PropertyType satementType, string referenceFieldName, string category, string description, bool browsable)
        {
            CodeMemberProperty property = new CodeMemberProperty();
            property.Name = propName;
            property.Type = new CodeTypeReference(propTypeName);
            property.Attributes = attribute;

            if (satementType.Equals(PropertyType.GET))
            {
                if (referenceFieldName != null && referenceFieldName.Length > 0)
                {
                    //property.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression("aaa")));
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), referenceFieldName)));
                }
                //else
                //{
                //    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "myname")));

                //}
            }
            else if (satementType.Equals(PropertyType.SET))
            {
                if (referenceFieldName != null && referenceFieldName.Length > 0)
                {
                    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), referenceFieldName), new CodePropertySetValueReferenceExpression()));
                }
                //else
                //{
                //    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "myname"), new CodePropertySetValueReferenceExpression()));

                //}
            }
            else
            {
                if (referenceFieldName != null && referenceFieldName.Length > 0)
                {
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), referenceFieldName)));
                    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), referenceFieldName), new CodePropertySetValueReferenceExpression()));
                }
            }

            CodeAttributeArgument[] arguments = null;
            CodeAttributeDeclaration propertyLevelAttribute = null;

            if (browsable)
            {
                arguments = new CodeAttributeArgument[1];
                arguments[0] = new CodeAttributeArgument(new CodeSnippetExpression("true"));
                propertyLevelAttribute = new CodeAttributeDeclaration(new CodeTypeReference("Browsable"), arguments);//Create attribute to be added to method
                property.CustomAttributes.Add(propertyLevelAttribute);//Add attribute to method
            }

            //Category
            if (category != null && category.Length > 0)
            {
                arguments = new CodeAttributeArgument[1];
                arguments = new CodeAttributeArgument[1];
                arguments[0] = new CodeAttributeArgument(new CodePrimitiveExpression(category));
                propertyLevelAttribute = new CodeAttributeDeclaration(new CodeTypeReference("Category"), arguments);//Create attribute to be added to method
                property.CustomAttributes.Add(propertyLevelAttribute);//Add attribute to method
            }

            //Display Name
            if (displayName != null && displayName.Length > 0)
            {
                arguments = new CodeAttributeArgument[1];
                arguments = new CodeAttributeArgument[1];
                arguments[0] = new CodeAttributeArgument(new CodePrimitiveExpression(displayName));
                propertyLevelAttribute = new CodeAttributeDeclaration(new CodeTypeReference("DisplayName"), arguments);
                property.CustomAttributes.Add(propertyLevelAttribute);
            }

            //Description
            if (description != null && description.Length > 0)
            {
                arguments = new CodeAttributeArgument[1];
                arguments = new CodeAttributeArgument[1];
                arguments[0] = new CodeAttributeArgument(new CodePrimitiveExpression(description));
                propertyLevelAttribute = new CodeAttributeDeclaration(new CodeTypeReference("Description"), arguments);
                property.CustomAttributes.Add(propertyLevelAttribute);
            }

            derived.Members.Add(property);

            return property;
        }

        #endregion


        #region Generate Code

        public string GeneratorCode()
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeGenerator codeGenerator = codeProvider.CreateGenerator();

            StringBuilder generatedCode = new StringBuilder();
            using (StringWriter codeWriter = new StringWriter(generatedCode))
            {
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";//Keep the braces on the line following the statement or declaration that they are associated with
                codeGenerator.GenerateCodeFromCompileUnit(unit, codeWriter, options);
            }

            this._code = generatedCode.ToString();
            return _code;
        }

        #endregion

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }


        public object GetInstance()
        {
            return GetInstance(_nameSpace + "." + _className);
        }

        public object GetInstance(string typeName)
        {
            if (this._code == null && this._code.Length.Equals(0))
                return null;

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler codeCompiler = codeProvider.CreateCompiler();
            CompilerParameters parameters = new CompilerParameters();

            for (int i = 0; i < unit.ReferencedAssemblies.Count; i++)
            {
                //parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add(unit.ReferencedAssemblies[i]);
            }

            parameters.GenerateInMemory = false;

            CompilerResults results = codeCompiler.CompileAssemblyFromSource(parameters, this._code);

            if (results.Errors.HasErrors)
            {
                string errorMessage = "";
                errorMessage = results.Errors.Count.ToString() + " Errors:";
                for (int x = 0; x < results.Errors.Count; x++)
                {
                    errorMessage = errorMessage + "\r\nLine: " + results.Errors[x].Line.ToString() + " - " + results.Errors[x].ErrorText;
                }

                return errorMessage;
            }
            else
            {
                //object ob = results.CompiledAssembly.CreateInstance("WindowsFormsApplication1.Derived");
                object ob = results.CompiledAssembly.CreateInstance(typeName);

                return ob;
            }

            return null;
        }



        #region GetClass

        public void AddClass(CodeTypeDeclaration classcode)
        {
            customEntityRoot.Types.Add(classcode);
        }


        public CodeTypeDeclaration GetClass()
        {
            return this.derived;
        }

        #endregion
    }
}