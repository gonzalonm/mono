//
// BuildChoose.cs
//
// Author:
//   Jonathan Chambers (joncham@gmail.com)
//
// (C) 2009 Jonathan Chambers
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using Microsoft.Build.BuildEngine;
using NUnit.Framework;

namespace MonoTests.Microsoft.Build.BuildEngine {
	[TestFixture]
	public class BuildChooseTest {

        [Test]
        [Category ("NotWorking")]
        public void TestEmptyWhen () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When Condition=""'$(Configuration)' == ''"">
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </When>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);

            Assert.AreEqual (1, project.ItemGroups.Count, "A1");
            Assert.AreEqual (0, project.PropertyGroups.Count, "A2");
            Assert.AreEqual (1, project.EvaluatedItems.Count, "A3");
            Assert.AreEqual (1, project.EvaluatedItemsIgnoringCondition.Count, "A4");
        }

        [Test]
        [Category ("NotWorking")]
        public void TestFalseWhen () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When Condition=""'$(Configuration)' == 'False'"">
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </When>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);

            Assert.AreEqual (1, project.ItemGroups.Count, "A1");
            Assert.AreEqual (0, project.PropertyGroups.Count, "A2");
            Assert.AreEqual (0, project.EvaluatedItems.Count, "A3");
            Assert.AreEqual (0, project.EvaluatedItemsIgnoringCondition.Count, "A4");
        }

        [Test]
        [Category ("NotWorking")]
        public void TestMultipleTrueWhen () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When Condition=""'$(Configuration)' == ''"">
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </When>
                        <When Condition=""'$(Configuration)' == ''"">
					        <ItemGroup>
						        <B Include='a' />
					        </ItemGroup>
                        </When>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);


            Assert.AreEqual (2, project.ItemGroups.Count, "A1");
            Assert.AreEqual (0, project.PropertyGroups.Count, "A2");
            Assert.AreEqual (1, project.EvaluatedItems.Count, "A3");
            Assert.AreEqual ("A", project.EvaluatedItems[0].Name, "A4");
            Assert.AreEqual (1, project.EvaluatedItemsIgnoringCondition.Count, "A5");
        }

        [Test]
        [Category ("NotWorking")]
        public void TestMultipleFalseWhen () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When Condition=""'$(Configuration)' == 'False'"">
				            <ItemGroup>
					            <A Include='a' />
				            </ItemGroup>
                        </When>
                        <When Condition=""'$(Configuration)' == 'False'"">
				            <ItemGroup>
					            <B Include='a' />
				            </ItemGroup>
                        </When>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);

            Assert.AreEqual (2, project.ItemGroups.Count, "A1");
            Assert.AreEqual (0, project.PropertyGroups.Count, "A2");
            Assert.AreEqual (0, project.EvaluatedItems.Count, "A3");
            Assert.AreEqual (0, project.EvaluatedItemsIgnoringCondition.Count, "A4");
        }

        [Test]
        [Category ("NotWorking")]
        [ExpectedException (typeof (InvalidProjectFileException))]
        public void TestMissingWhen () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            // a <Choose> requires at least one <When>
            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <Otherwise>
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </Otherwise>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);
        }

        [Test]
        [Category ("NotWorking")]
        [ExpectedException (typeof (InvalidProjectFileException))]
        public void TestMissingConditionWhen () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            // a <When> requires a Condition
            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When>
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </When>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);
        }

        [Test]
        [Category ("NotWorking")]
        public void TestWhenOtherwise1 () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When Condition=""'$(Configuration)' == ''"">
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </When>
                        <Otherwise>
					        <ItemGroup>
						        <B Include='a' />
					        </ItemGroup>
                        </Otherwise>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);

            Assert.AreEqual (2, project.ItemGroups.Count, "A1");
            Assert.AreEqual (0, project.PropertyGroups.Count, "A2");
            Assert.AreEqual (1, project.EvaluatedItems.Count, "A3");
            Assert.AreEqual ("A", project.EvaluatedItems[0].Name, "A4");
            Assert.AreEqual (1, project.EvaluatedItemsIgnoringCondition.Count, "A5");
        }

        [Test]
        [Category ("NotWorking")]
        public void TestWhenOtherwise2 () {
            Engine engine;
            Project project;
            BuildItemGroup[] groups = new BuildItemGroup[1];

            string documentString = @"
				<Project xmlns='http://schemas.microsoft.com/developer/msbuild/2003'>
                    <Choose>
                        <When Condition=""'$(Configuration)' == 'False'"">
					        <ItemGroup>
						        <A Include='a' />
					        </ItemGroup>
                        </When>
                        <Otherwise>
					        <ItemGroup>
						        <B Include='a' />
					        </ItemGroup>
                        </Otherwise>
                    </Choose>
				</Project>
			";

            engine = new Engine (Consts.BinPath);
            project = engine.CreateNewProject ();
            project.LoadXml (documentString);

            Assert.AreEqual (2, project.ItemGroups.Count, "A1");
            Assert.AreEqual (0, project.PropertyGroups.Count, "A2");
            Assert.AreEqual (1, project.EvaluatedItems.Count, "A3");
            Assert.AreEqual ("B", project.EvaluatedItems[0].Name, "A4");
            Assert.AreEqual (1, project.EvaluatedItemsIgnoringCondition.Count, "A5");
        }
	}
}
