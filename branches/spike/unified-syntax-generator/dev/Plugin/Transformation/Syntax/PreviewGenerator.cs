using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Syntax preview generator. Converts a langauge element into an HTML snippet
	/// that can be inserted into a documentation preview.
	/// </summary>
	/// <remarks>
	/// <para>
	/// CSS classes for common syntax generator:
	/// </para>
	/// <para>
	/// <c>.code</c> - wrapper around code blocks
	/// </para>
	/// <para>
	/// Language constructs:
	/// </para>
	/// <list type="bullet">
	/// <item><term>.attribute</term></item>
	/// <item><term>.comment</term></item>
	/// <item><term>.identifier</term></item>
	/// <item><term>.keyword</term></item>
	/// <item><term>.literal</term></item>
	/// <item><term>.member</term></item>
	/// <item><term>.parameter</term></item>
	/// <item><term>.parameters</term></item>
	/// <item><term>.typeparameter</term></item>
	/// <item><term>.typeparameters</term></item>
	/// </list>
	/// <para>
	/// There will be line continuations and line breaks in places. That may
	/// not quite be identical to how the finished product actually renders
	/// but there's not much we can do about that since we can't get CSS
	/// to add the special line-continuation characters in VB.
	/// </para>
	/// <para>
	/// Since we're not generating the preview for every language all at once,
	/// there is no need for CSS classes that are language-specific.
	/// </para>
	/// </remarks>
	/// <example>
	/// <para>
	/// For this method:
	/// </para>
	/// <code lang="C#">
	/// [SomeAttribute]
	/// [OtherAttribute(1, b=2)]
	/// public abstract string MyMethod&lt;T&gt;(out string a, T b)
	/// </code>
	/// <para>
	/// The corresponding HTML for a C# preview would be:
	/// </para>
	/// <code>
	/// &lt;div class=&quot;code&quot;&gt;
	///   &lt;div class=&quot;attribute&quot;&gt;[SomeAttribute]&lt;/div&gt;
	///   &lt;div class=&quot;attribute&quot;&gt;[OtherAttribute&lt;div class=&quot;parameters&quot;&gt;(&lt;div class=&quot;literal&quot;&gt;1&lt;/div&gt;, &lt;div class=&quot;parameter&quot;&gt;b&lt;/div&gt;=&lt;div class=&quot;literal&quot;&gt;2&lt;/div&gt;)&lt;/div&gt;]&lt;/div&gt;
	///   &lt;div class=&quot;member&quot;&gt;
	///     &lt;div class=&quot;keyword&quot;&gt;public&lt;div&gt;
	///     &lt;div class=&quot;keyword&quot;&gt;abstract&lt;/div&gt;
	///     &lt;div class=&quot;keyword&quot;&gt;&lt;a href=&quot;#&quot;&gt;string&lt;/a&gt;&lt;/div&gt;
	///     &lt;div class=&quot;identifier&quot;&gt;MyMethod&lt;/div&gt;&lt;div class=&quot;typeparameters&quot;&gt;&amp;lt;&lt;div class=&quot;typeparameter&quot;&gt;T&lt;/div&gt;&amp;gt;&lt;/div&gt;&lt;div class=&quot;parameters&quot;&gt;(&lt;br /&gt;
	///     &lt;div class=&quot;keyword&quot;&gt;out&lt;/div&gt; &lt;div class=&quot;keyword&quot;&gt;&lt;a href=&quot;#&quot;&gt;string&lt;/a&gt;&lt;/div&gt; &lt;div class=&quot;parameter&quot;&gt;a&lt;/div&gt;,&lt;br /&gt;
	///     &lt;div class=&quot;keyword&quot;&gt;T&lt;/div&gt; &lt;div class=&quot;parameter&quot;&gt;b&lt;/div&gt;&lt;br /&gt;
	///     )&lt;/div&gt;
	///   &lt;/div&gt;
	/// &lt;/div&gt;
	/// </code>
	/// </example>
	public class PreviewGenerator
	{
		/* 
		 * Sample:
		 * <div class="code">
		 *   <div class="attribute">[SomeAttribute]</div>
		 *   <div class="attribute">[OtherAttribute<div class="parameters">(<div class="parameter">1</div>, <div class="parameter">b</div>=<div class="literal">2</div>)</div>]</div>
		 *   <div class="member">
		 *     <div class="keyword">public<div>
		 *     <div class="keyword">abstract</div>
		 *     <div class="keyword"><a href="#">string</a></div>
		 *     <div class="identifier">MyMethod</div><div class="typeparameters">&lt;<div class="typeparameter">T</div>&gt;</div><div class="parameters">(<br />
		 *     <div class="keyword">out</div> <div class="keyword"><a href="#">string</a></div> <div class="parameter">a</div>,<br />
		 *     <div class="keyword"><a href="#">string</a></div> <div class="parameter">b</div><br />
		 *     )</div>
		 *   </div>
		 * </div>
		 */

	}
}
