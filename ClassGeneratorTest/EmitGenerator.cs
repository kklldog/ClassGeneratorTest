using System.Reflection.Emit;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassGeneratorTest
{
    public class EmitGenerator
    {
        public Type GenerateType()
        {
            MethodAttributes getSetAttr =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig;

            // define class
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("ClassGeneratorTest.Entities"), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("ClassGeneratorTest.Entities");
            var typeBuilder = moduleBuilder.DefineType("entity_app_class", TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.AutoClass | TypeAttributes.AnsiClass);

            //define fields
            var idFieldBuilder = typeBuilder.DefineField("_id",
                                                        typeof(string),
                                                        FieldAttributes.Private);
            var nameFieldBuilder = typeBuilder.DefineField("_name",
                                                       typeof(string),
                                                       FieldAttributes.Private);
            //define property
            var idProp = typeBuilder.DefineProperty("Id", PropertyAttributes.SpecialName, typeof(string), Type.EmptyTypes);
            var nameProp = typeBuilder.DefineProperty("Name", PropertyAttributes.SpecialName, typeof(string), Type.EmptyTypes);

            //define getter setter
            var idGetPropMthdBldr = typeBuilder.DefineMethod("get_id",getSetAttr,typeof(string),Type.EmptyTypes);
            var idGetIL = idGetPropMthdBldr.GetILGenerator();
            idGetIL.Emit(OpCodes.Ldarg_0);
            idGetIL.Emit(OpCodes.Ldfld, idFieldBuilder);
            idGetIL.Emit(OpCodes.Ret);
            var idSetPropMthdBldr = typeBuilder.DefineMethod("set_id", getSetAttr, null, new Type[] { typeof(string) });
            var idSetIL = idSetPropMthdBldr.GetILGenerator();
            idSetIL.Emit(OpCodes.Ldarg_0);
            idSetIL.Emit(OpCodes.Ldarg_1);
            idSetIL.Emit(OpCodes.Stfld, idFieldBuilder);
            idSetIL.Emit(OpCodes.Ret);
            // connect prop to getter setter
            idProp.SetGetMethod(idGetPropMthdBldr);
            idProp.SetSetMethod(idSetPropMthdBldr);

            var nameGetPropMthdBldr = typeBuilder.DefineMethod("get_name", getSetAttr, typeof(string), Type.EmptyTypes);
            var nameGetIL = nameGetPropMthdBldr.GetILGenerator();
            nameGetIL.Emit(OpCodes.Ldarg_0);
            nameGetIL.Emit(OpCodes.Ldfld, nameFieldBuilder);
            nameGetIL.Emit(OpCodes.Ret);
            var nameSetPropMthdBldr = typeBuilder.DefineMethod("set_name", getSetAttr, null, new Type[] { typeof(string) });
            var nameSetIL = nameSetPropMthdBldr.GetILGenerator();
            nameSetIL.Emit(OpCodes.Ldarg_0);
            nameSetIL.Emit(OpCodes.Ldarg_1);
            nameSetIL.Emit(OpCodes.Stfld, nameFieldBuilder);
            nameSetIL.Emit(OpCodes.Ret);
            // connect prop to getter setter
            nameProp.SetGetMethod(nameGetPropMthdBldr);
            nameProp.SetSetMethod(nameSetPropMthdBldr);

            // set attrbutes
            Type[] attributeParams = new Type[] { typeof(string) };
            ConstructorInfo attrCtorInfo = typeof(TableAttribute).GetConstructor(attributeParams);
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { "app" });
            typeBuilder.SetCustomAttribute(attributeBuilder);

            //create type
            var type = typeBuilder.CreateType();

            return type;
        }

        public object EfTest()
        {
            var dynamicType = GenerateType();
            using var db = new DB(dynamicType);
            var dbset = db.GetType().GetMethods().Where(x => x.Name == "Set").First().MakeGenericMethod(dynamicType);
            dynamic query = dbset.Invoke(db, null);
            var resultSet = (query as IEnumerable<object>).ToList();

            return resultSet;
        }

    }
}