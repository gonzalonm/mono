thisdir = class/System.Web.Extensions
SUBDIRS = Test
include ../../build/rules.make

LIBRARY = System.Web.Extensions.dll

RESOURCE_FILES= \
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjax.debug.js	\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjax.js		\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxTimer.debug.js	\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxTimer.js		\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxWebForms.debug.js\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxWebForms.js

RESOURCE_FILES_DIST= \
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjax.debug.js	\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjax.js		\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxTimer.debug.js	\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxTimer.js		\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxWebForms.debug.js\
	../MicrosoftAjaxLibrary/System.Web.Extensions/3.5.0.0/3.5.21022.8/MicrosoftAjaxWebForms.js\
	../MicrosoftAjaxLibrary/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.debug.js	\
	../MicrosoftAjaxLibrary/System.Web.Extensions/1.0.61025.0/MicrosoftAjax.js		\
	../MicrosoftAjaxLibrary/System.Web.Extensions/1.0.61025.0/MicrosoftAjaxTimer.debug.js	\
	../MicrosoftAjaxLibrary/System.Web.Extensions/1.0.61025.0/MicrosoftAjaxTimer.js		\
	../MicrosoftAjaxLibrary/System.Web.Extensions/1.0.61025.0/MicrosoftAjaxWebForms.debug.js\
	../MicrosoftAjaxLibrary/System.Web.Extensions/1.0.61025.0/MicrosoftAjaxWebForms.js \
	Resources/MicrosoftAjaxExtension.js \
	Resources/MicrosoftAjaxWebFormsExtension.js

NUNIT_RESOURCE_FILES= \
	Test/resources/Web.mono.config \
	Test/resources/profile.config.4.x \
	$(wildcard Test/resources/*.as?x) \
	$(wildcard ../System.Web/Test/mainsoft/NunitWeb/NunitWeb/Resources/*.cs) \
	$(wildcard ../System.Web/Test/mainsoft/NunitWeb/NunitWeb/Resources/*.as?x) \
	$(wildcard ../System.Web/Test/mainsoft/NunitWeb/NunitWeb/Resources/*.master)

CLASSLIB_DIR = $(topdir)/class/lib/$(PROFILE)

STANDALONE_RUNNER_SUPPORT_ASSEMBLY = $(CLASSLIB_DIR)/standalone-runner-support.dll
STANDALONE_TEST_MCS_FLAGS = $(LIB_MCS_FLAGS) $(PROFILE_MCS_FLAGS) -r:$(STANDALONE_RUNNER_SUPPORT_ASSEMBLY) -r:$(topdir)/class/lib/$(PROFILE)/System.Web.dll -r:$(topdir)/class/lib/$(PROFILE)/System.Web.Extensions.dll -r:$(topdir)/class/lib/$(PROFILE)/nunit.framework.dll
STANDALONE_TEST_ASSEMBLY = System.Web.Extensions_standalone_test_$(PROFILE).dll
STANDALONE_TEST_MAKEFRAG = $(depsdir)/$(STANDALONE_TEST_ASSEMBLY).makefrag

STANDALONE_TEST_RUNNER = ../System.Web/Test/tools/standalone-runner.exe
RUN_STANDALONE = $(TEST_RUNTIME) $(STANDALONE_TEST_RUNNER)
ifdef TESTNAME
RUN_STANDALONE += --test=$(TESTNAME)
endif

LIB_REFS = System System.Core System.Drawing System.Data System.Data.Linq System.Xml System.Web System.Web.Services System.Configuration System.EnterpriseServices System.ServiceModel
LIB_MCS_FLAGS = \
	-unsafe \
	-define:NET_3_5	-nowarn:436		\
	-define:SYSTEM_WEB_EXTENSIONS	\
	$(OTHER_LIB_MCS_FLAGS) 		\
	$(RESOURCE_FILES:%=/resource:%)

ifeq (4, $(FRAMEWORK_VERSION_MAJOR))
LIB_REFS += System.Web.ApplicationServices
endif

TEST_LIB_REFS = SystemWebTestShim
TEST_MCS_FLAGS = $(LIB_MCS_FLAGS) -doc:$(test_lib:.dll=.xml) -nowarn:219,169,1591 $(NUNIT_RESOURCE_FILES:%=/resource:%) -define:SYSTEM_WEB_EXTENSIONS

EXTRA_DISTFILES = $(RESOURCE_FILES_DIST) $(NUNIT_RESOURCE_FILES) \
	System.Web.Extensions_standalone_test.dll.sources \
	Test/standalone-tests/Consts.cs.in \
	Test/standalone-tests/DataPager_NumericPagerField_Bug615315.cs

build-standalone-runner-support:
	$(MAKE) -C ../System.Web/ standalone-runner-support
	$(MAKE) -C ../System.Web/Test/tools/ STANDALONE_SUPPORT_BUILT=1 standalone-runner.exe

run-standalone-test: build-standalone-runner-support $(STANDALONE_RUNNER_SUPPORT_MAKEFRAG) $(STANDALONE_TEST_MAKEFRAG) $(STANDALONE_TEST_ASSEMBLY)
	$(RUN_STANDALONE) $(STANDALONE_TEST_ASSEMBLY)

$(STANDALONE_TEST_MAKEFRAG): System.Web.Extensions_standalone_test.dll.sources
	@echo Creating $@ ...
	@sed 's,^,$(STANDALONE_TEST_ASSEMBLY): ,' $< > $@

include $(STANDALONE_TEST_MAKEFRAG)

$(STANDALONE_TEST_ASSEMBLY): $(the_assembly) Test/standalone-tests/Consts.cs
	$(CSCOMPILE) $(STANDALONE_TEST_MCS_FLAGS) -out:$@ -target:library @System.Web.Extensions_standalone_test.dll.sources

Test/standalone-tests/Consts.cs: Test/standalone-tests/Consts.cs.in
ifeq ($(PLATFORM), win32)
	@sed 's,@SystemWebExtensionsClassDir@,$(shell cygpath -a -m .),' $< > $@
else
	@sed 's,@SystemWebExtensionsClassDir@,$(shell pwd),' $< > $@
endif

include ../../build/library.make

