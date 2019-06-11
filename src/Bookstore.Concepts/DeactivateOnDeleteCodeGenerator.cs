using System;
using System.ComponentModel.Composition;
using Rhetos.Compiler;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Dsl;
using Rhetos.Extensibility;

namespace Bookstore.Concepts
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(DeactivateOnDeleteInfo))]
    public class DeactivateOnDeleteCodeGenerator : IConceptCodeGenerator
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (DeactivateOnDeleteInfo)conceptInfo;

            var code = String.Format(
            @"var deactivated = deleted.ToList();

            foreach(var item in deleted)
                item.Active = false;

            updated = updated.Concat(deleted).ToArray();
            updatedNew = updatedNew.Concat(deleted).ToArray();

            deleted = new Common.Queryable.{0}_{1}[]{{}};
            deletedIds = new {0}.{1}[]{{}};
            ",
                info.Deactivatable.Entity.Module.Name,
                info.Deactivatable.Entity.Name);

            codeBuilder.InsertCode(code, WritableOrmDataStructureCodeGenerator.OldDataLoadedTag, info.Deactivatable.Entity);
        }
    }
}
