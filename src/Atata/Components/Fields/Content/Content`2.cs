﻿namespace Atata
{
    /// <summary>
    /// Represents any HTML element containing content. By default is being searched by the label (if is declared in the class inherited from `TableRow`, then by column header).
    /// </summary>
    /// <typeparam name="T">The type of the content.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition(ComponentTypeName = "element")]
    [ControlFinding(FindTermBy.ColumnHeader, ParentComponentType = typeof(TableRow<>))]
    public class Content<T, TOwner> : Field<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string DataProviderName
        {
            get { return "content"; }
        }

        protected override T GetValue()
        {
            string value = Scope.Text;
            return ConvertStringToValue(value);
        }
    }
}
