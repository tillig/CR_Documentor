using System;

namespace DocTestCS.Tags.Inline
{
	/// <summary>
	/// This class tests the 'list' tag.
	/// </summary>
	public class List
	{
		#region Bullet

		/// <summary>
		/// Bulleted list with header:
		/// <list type="bullet">
		/// <listheader>
		/// <term>Header Term</term>
		/// <description>Header Description</description>
		/// </listheader>
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void BulletHeader()
		{
		}

		/// <summary>
		/// Bulleted list with no header:
		/// <list type="bullet">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void BulletNoHeader()
		{
		}

		/// <summary>
		/// Bulleted list with descriptions only:
		/// <list type="bullet">
		/// <item>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void BulletDescriptionOnly()
		{
		}

		/// <summary>
		/// Bulleted list with terms only:
		/// <list type="bullet">
		/// <item>
		/// <term>Item 1 Term</term>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// </item>
		/// </list>
		/// </summary>
		public void BulletTermOnly()
		{
		}

		/// <summary>
		/// Bulleted list with a nested list in the term:
		/// <list type="bullet">
		/// <item>
		/// <term>Item 1 Term
		/// <list type="bullet">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void BulletNestedTerm()
		{
		}

		/// <summary>
		/// Bulleted list with a nested list in the description:
		/// <list type="bullet">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description
		/// <list type="bullet">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void BulletNestedDescription()
		{
		}

		#endregion

		#region Number

		/// <summary>
		/// Numbered list with header:
		/// <list type="number">
		/// <listheader>
		/// <term>Header Term</term>
		/// <description>Header Description</description>
		/// </listheader>
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void NumberHeader()
		{
		}

		/// <summary>
		/// Numbered list with no header:
		/// <list type="number">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void NumberNoHeader()
		{
		}

		/// <summary>
		/// Numbered list with descriptions only:
		/// <list type="number">
		/// <item>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void NumberDescriptionOnly()
		{
		}

		/// <summary>
		/// Numbered list with terms only:
		/// <list type="number">
		/// <item>
		/// <term>Item 1 Term</term>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// </item>
		/// </list>
		/// </summary>
		public void NumberTermOnly()
		{
		}

		/// <summary>
		/// Numbered list with a nested list in the term:
		/// <list type="number">
		/// <item>
		/// <term>Item 1 Term
		/// <list type="number">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void NumberNestedTerm()
		{
		}

		/// <summary>
		/// Numbered list with a nested list in the description:
		/// <list type="number">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description
		/// <list type="number">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void NumberNestedDescription()
		{
		}

		#endregion

		#region Table

		/// <summary>
		/// Table with header:
		/// <list type="table">
		/// <listheader>
		/// <term>Header Term</term>
		/// <description>Header Description</description>
		/// </listheader>
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void TableHeader()
		{
		}

		/// <summary>
		/// Table with no header:
		/// <list type="table">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void TableNoHeader()
		{
		}

		/// <summary>
		/// Table with descriptions only:
		/// <list type="table">
		/// <item>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void TableDescriptionOnly()
		{
		}

		/// <summary>
		/// Table with terms only:
		/// <list type="table">
		/// <item>
		/// <term>Item 1 Term</term>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// </item>
		/// </list>
		/// </summary>
		public void TableTermOnly()
		{
		}

		/// <summary>
		/// Table with a nested list in the term:
		/// <list type="table">
		/// <item>
		/// <term>Item 1 Term
		/// <list type="table">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void TableNestedTerm()
		{
		}

		/// <summary>
		/// Table with a nested list in the description:
		/// <list type="table">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description
		/// <list type="table">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void TableNestedDescription()
		{
		}

		#endregion

		#region Definition

		/// <summary>
		/// Definition list with header:
		/// <list type="definition">
		/// <listheader>
		/// <term>Header Term</term>
		/// <description>Header Description</description>
		/// </listheader>
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void DefinitionHeader()
		{
		}

		/// <summary>
		/// Definition list with no header:
		/// <list type="definition">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void DefinitionNoHeader()
		{
		}

		/// <summary>
		/// Definition list with descriptions only:
		/// <list type="definition">
		/// <item>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void DefinitionDescriptionOnly()
		{
		}

		/// <summary>
		/// Definition list with terms only:
		/// <list type="definition">
		/// <item>
		/// <term>Item 1 Term</term>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// </item>
		/// </list>
		/// </summary>
		public void DefinitionTermOnly()
		{
		}

		/// <summary>
		/// Definition list with a nested list in the term:
		/// <list type="definition">
		/// <item>
		/// <term>Item 1 Term
		/// <list type="definition">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </term>
		/// <description>Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void DefinitionNestedTerm()
		{
		}

		/// <summary>
		/// Definition list with a nested list in the description:
		/// <list type="definition">
		/// <item>
		/// <term>Item 1 Term</term>
		/// <description>Item 1 Description
		/// <list type="definition">
		/// <item>
		/// <term>Nested Item 1 Term</term>
		/// <description>Nested Item 1 Description</description>
		/// </item>
		/// <item>
		/// <term>Nested Item 2 Term</term>
		/// <description>Nested Item 2 Description</description>
		/// </item>
		/// </list>
		/// </description>
		/// </item>
		/// <item>
		/// <term>Item 2 Term</term>
		/// <description>Item 2 Description</description>
		/// </item>
		/// </list>
		/// </summary>
		public void DefinitionNestedDescription()
		{
		}

		#endregion

	}
}
