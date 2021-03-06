*** Settings ***
Resource    ../../libs/admin-common.robot
Resource    ../../libs/public-common.robot
Library     ../../libs/admin_api.py

Force Tags  Admin  Local  Dev  AltersData

Suite Setup       user signs in as bau1
Suite Teardown    user closes the browser

*** Variables ***
${TOPIC_NAME}        %{TEST_TOPIC_NAME}
${PUBLICATION_NAME}  UI tests - publish data %{RUN_IDENTIFIER}
${SUBJECT_NAME}   UI test subject

*** Test Cases ***
Create new publication and release via API
    [Tags]  HappyPath
    ${PUBLICATION_ID}=  user creates test publication via api   ${PUBLICATION_NAME}
    user create test release via api  ${PUBLICATION_ID}   FY   3000

Navigate to release
    [Tags]  HappyPath
    user navigates to release summary from admin dashboard  ${PUBLICATION_NAME}  Financial Year 3000-01 (not Live)

Add public prerelease access list
    [Tags]  HappyPath
    user clicks link  Pre-release access
    user waits until h2 is visible  Manage pre-release user access

    user clicks link  Public access list
    user waits until h2 is visible  Public pre-release access list
    user clicks button   Create public pre-release access list
    user presses keys  CTRL+a+BACKSPACE
    user presses keys  Test public access list
    user clicks button  Save access list
    user waits until element contains  css:[data-testid="publicPreReleaseAccessListPreview"]  Test public access list

Go to "Sign off" page
    [Tags]  HappyPath
    user clicks link   Sign off
    user waits until h2 is visible  Sign off
    user waits until page contains button  Edit release status

Approve release
    [Tags]  HappyPath
    user clicks button  Edit release status
    user waits until h2 is visible  Edit release status

    user clicks radio   Approved for publication
    user enters text into element  id:releaseStatusForm-internalReleaseNote  Approved by UI tests - latest release
    user clicks radio  As soon as possible

    user clicks button   Update status

Verify release is scheduled
    [Tags]  HappyPath
    user waits until h2 is visible  Sign off
    user checks summary list contains  Current status  Approved

Wait for release process status to be Complete
    [Tags]  HappyPath
    user waits for release process status to be  Complete    ${release_complete_wait}
    user reloads page  # EES-1448
    user checks page does not contain button  Edit release status

Return to Admin Dashboard
    [Tags]  HappyPath
    user goes to url    %{ADMIN_URL}
    user waits until h1 is visible   Dashboard
    user waits until page contains element   id:publicationsReleases-themeTopic-themeId    180

Create another release for the same publication
    [Tags]  HappyPath
    user selects theme and topic from admin dashboard  %{TEST_THEME_NAME}  ${TOPIC_NAME}
    user waits until page contains link    Create new publication
    user opens accordion section   ${PUBLICATION_NAME}
    user clicks testid element   Create new release link for ${PUBLICATION_NAME}
    user creates release for publication  ${PUBLICATION_NAME}  Financial Year  3001

Verify new release summary
    [Tags]  HappyPath
    user checks page contains element   xpath://li/a[text()="Summary" and contains(@aria-current, 'page')]
    user waits until h2 is visible  Release summary
    user checks summary list contains  Publication title  ${PUBLICATION_NAME}

Upload subject to new release
    [Tags]  HappyPath
    user clicks link  Data and files
    user waits until page contains element  id:dataFileUploadForm-subjectTitle
    user enters text into element  id:dataFileUploadForm-subjectTitle   ${SUBJECT_NAME}
    user chooses file   id:dataFileUploadForm-dataFile       ${FILES_DIR}upload-file-test.csv
    user chooses file   id:dataFileUploadForm-metadataFile   ${FILES_DIR}upload-file-test.meta.csv
    user clicks button  Upload data files

    user waits until h2 is visible  Uploaded data files
    user waits until page contains accordion section   ${SUBJECT_NAME}
    user opens accordion section   ${SUBJECT_NAME}

    ${section}=  user gets accordion section content element  ${SUBJECT_NAME}
    user checks headed table body row contains  Subject title    ${SUBJECT_NAME}  ${section}
    user checks headed table body row contains  Data file        upload-file-test.csv  ${section}
    user checks headed table body row contains  Metadata file    upload-file-test.meta.csv  ${section}
    user checks headed table body row contains  Number of rows   159  ${section}
    user checks headed table body row contains  Data file size   15 Kb  ${section}
    user checks headed table body row contains  Status           Complete  ${section}  360

Add meta guidance to subject
    [Tags]  HappyPath
    user clicks link  Metadata guidance
    user waits until h2 is visible  Public metadata guidance document

    user waits until page contains accordion section  ${SUBJECT_NAME}
    user opens accordion section  ${SUBJECT_NAME}
    user enters text into meta guidance data file content editor  ${SUBJECT_NAME}
    ...  ${SUBJECT_NAME} meta guidance content
    user clicks button  Save guidance

Navigate to 'Create data block' page
    [Tags]  HappyPath
    user clicks link    Data blocks

    user waits until h2 is visible  Data blocks
    user clicks link  Create data block

    user waits until h2 is visible  Create data block
    user waits until h2 is visible   Choose a subject

Select subject "${SUBJECT_NAME}"
    [Tags]  HappyPath
    user waits until page contains   ${SUBJECT_NAME}
    user clicks radio    ${SUBJECT_NAME}
    user clicks element   id:publicationSubjectForm-submit

Select locations
    [Tags]   HappyPath
    user waits until h2 is visible  Choose locations
    user opens details dropdown   Opportunity Area
    user clicks checkbox   Bolton 001 (E02000984)
    user clicks checkbox   Bolton 001 (E05000364)
    user clicks checkbox   Bolton 004 (E02000987)
    user clicks checkbox   Bolton 004 (E05010450)
    user opens details dropdown   Ward
    user clicks checkbox   Nailsea Youngwood
    user clicks checkbox   Syon
    user clicks element     id:locationFiltersForm-submit

Select time period
    [Tags]   HappyPath
    user waits until h2 is visible  Choose time period
    user selects from list by label  id:timePeriodForm-start  2005
    user selects from list by label  id:timePeriodForm-end  2020
    user clicks element     id:timePeriodForm-submit

Select indicators
    [Tags]  HappyPath
    user waits until h2 is visible  Choose your filters
    user clicks indicator checkbox    Admission Numbers

Create table
    [Tags]  HappyPath
    user clicks element   id:filtersForm-submit
    user waits until results table appears     180

Save data block as a highlight
    [Tags]  HappyPath
    user enters text into element  id:dataBlockDetailsForm-name         UI Test data block name
    user enters text into element  id:dataBlockDetailsForm-heading      UI Test table title
    user enters text into element  id:dataBlockDetailsForm-source       UI Test source

    user clicks checkbox  Set as a table highlight for this publication
    user waits until page contains element  id:dataBlockDetailsForm-highlightName
    user enters text into element  id:dataBlockDetailsForm-highlightName    Test highlight name
    user enters text into element  id:dataBlockDetailsForm-highlightDescription    Test highlight description

    user clicks button   Save data block
    user waits until page contains    Delete this data block

Add public prerelease access list for new release
    [Tags]  HappyPath
    user clicks link  Pre-release access
    user waits until h2 is visible  Manage pre-release user access
    user creates public prerelease access list   Test public access list

Go to "Sign off" page for new release
    [Tags]  HappyPath
    user clicks link   Sign off
    user waits until h2 is visible  Sign off
    user waits until page contains button  Edit release status

Approve new release
    [Tags]  HappyPath
    ${PUBLISH_DATE_DAY}=  get current datetime  %-d
    ${PUBLISH_DATE_MONTH}=  get current datetime  %-m
    ${PUBLISH_DATE_MONTH_WORD}=  get current datetime  %B
    ${PUBLISH_DATE_YEAR}=  get current datetime  %Y
    set suite variable  ${PUBLISH_DATE_DAY}
    set suite variable  ${PUBLISH_DATE_MONTH}
    set suite variable  ${PUBLISH_DATE_MONTH_WORD}
    set suite variable  ${PUBLISH_DATE_YEAR}

    user clicks button  Edit release status
    user waits until h2 is visible  Edit release status

    user clicks radio   Approved for publication
    user enters text into element  id:releaseStatusForm-internalReleaseNote  Approved by UI tests
    user clicks radio  As soon as possible
    user enters text into element  id:releaseStatusForm-nextReleaseDate-month   12
    user enters text into element  id:releaseStatusForm-nextReleaseDate-year    3001

    user clicks button   Update status

Wait for release process status for new release to be Complete
    [Tags]  HappyPath
    # EES-1007 - Release process status doesn't automatically update
    user waits until h2 is visible  Sign off
    user checks summary list contains  Current status  Approved
    user checks summary list contains  Scheduled release  ${PUBLISH_DATE_DAY} ${PUBLISH_DATE_MONTH_WORD} ${PUBLISH_DATE_YEAR}
    user waits for release process status to be  Complete    ${release_complete_wait}
    user reloads page  # EES-1448
    user checks page does not contain button  Edit release status

User goes to public Find Statistics page
    [Tags]  HappyPath
    environment variable should be set   PUBLIC_URL
    user goes to url   %{PUBLIC_URL}/find-statistics
    user waits until h1 is visible  Find statistics and data
    user waits for page to finish loading

Verify newly published release is on Find Statistics page
    [Tags]  HappyPath
    user waits until page contains accordion section   %{TEST_THEME_NAME}
    user opens accordion section  %{TEST_THEME_NAME}
    user waits until accordion section contains text   %{TEST_THEME_NAME}   ${TOPIC_NAME}

    user opens details dropdown  ${TOPIC_NAME}
    user waits until details dropdown contains publication    ${TOPIC_NAME}  ${PUBLICATION_NAME}
    user checks publication bullet contains link   ${PUBLICATION_NAME}  View statistics and data
    user checks publication bullet contains link   ${PUBLICATION_NAME}  Create your own tables online
    user checks publication bullet does not contain link  ${PUBLICATION_NAME}   Statistics at DfE

Navigate to published release page
    [Tags]  HappyPath
    user clicks testid element   View stats link for ${PUBLICATION_NAME}
    user waits until h1 is visible   ${PUBLICATION_NAME}  90

Check latest release is correct
    [Tags]  HappyPath
    user waits until page contains title caption  Financial Year 3001-02
    user checks page contains   This is the latest data
    user checks page contains   See other releases (1)

    user opens details dropdown  See other releases (1)
    user checks page contains other release   Financial Year 3000-01
    user checks page does not contain other release   Financial Year 3001-02

    user clicks link   Financial Year 3000-01

Check other release is correct
    [Tags]  HappyPath
    user waits until page contains title caption  Financial Year 3000-01

    user waits until page contains link    View latest data: Financial Year 3001-02
    user checks page contains   See other releases (1)
    user checks page contains other release    Financial Year 3001-02
    user checks page does not contain other release   Financial Year 3000-01

Go to Table Tool page
    [Tags]  HappyPath
    user goes to url  %{PUBLIC_URL}/data-tables
    user waits until h1 is visible  Create your own tables online
    user waits for page to finish loading

Select publication in table tool
    [Tags]  HappyPath
    user opens details dropdown    %{TEST_THEME_NAME}
    user opens details dropdown    ${TOPIC_NAME}
    user clicks radio      ${PUBLICATION_NAME}
    user clicks element    id:publicationForm-submit
    user waits until h2 is visible   View a featured table or create your own
    user checks previous table tool step contains  1    Publication    ${PUBLICATION_NAME}

Select subject "${SUBJECT_NAME}" in table tool
    [Tags]  HappyPath
    user clicks link  Create your own table
    user waits until h3 is visible  Choose a subject

    user waits until page contains   ${SUBJECT_NAME}
    user clicks radio    ${SUBJECT_NAME}
    user clicks element   id:publicationSubjectForm-submit
    user waits until h2 is visible  Choose locations
    user checks previous table tool step contains  2    Subject    ${SUBJECT_NAME}

Select locations in table tool
    [Tags]   HappyPath
    user opens details dropdown   Local Authority
    user clicks checkbox   Barnsley
    user clicks checkbox   Birmingham
    user clicks element     id:locationFiltersForm-submit
    user waits until h2 is visible  Choose time period
    user checks previous table tool step contains  3   Local Authority    Barnsley
    user checks previous table tool step contains  3   Local Authority    Birmingham

Select time period in table tool
    [Tags]   HappyPath
    user selects from list by label  id:timePeriodForm-start  2014
    user selects from list by label  id:timePeriodForm-end    2018
    user clicks element     id:timePeriodForm-submit

Select indicators in table tool
    [Tags]  HappyPath
    user waits until h2 is visible  Choose your filters
    user clicks indicator checkbox    Admission Numbers
    user clicks element   id:filtersForm-submit

Validate table
    [Tags]  HappyPath
    user waits until results table appears  180
    user checks table column heading contains   1  1  2014
    user checks table column heading contains   1  2  2015
    user checks table column heading contains   1  3  2016
    user checks table column heading contains   1  4  2017
    user checks table column heading contains   1  5  2018

    ${row}=  user gets row number with heading  Barnsley
    user checks table cell in offset row contains  ${row}  0  1  9,854
    user checks table cell in offset row contains  ${row}  0  2  1,134
    user checks table cell in offset row contains  ${row}  0  3  7,419
    user checks table cell in offset row contains  ${row}  0  4  5,032
    user checks table cell in offset row contains  ${row}  0  5  8,123

    ${row}=  user gets row number with heading   Birmingham
    user checks table cell in offset row contains  ${row}  0  1  3,708
    user checks table cell in offset row contains  ${row}  0  2  9,303
    user checks table cell in offset row contains  ${row}  0  3  8,856
    user checks table cell in offset row contains  ${row}  0  4  8,530
    user checks table cell in offset row contains  ${row}  0  5  3,962

Select table highlight from subjects step
    [Tags]  HappyPath
    user clicks testid element  wizardStep-2-goToButton
    user waits until h1 is visible  Go back to previous step
    user clicks button  Confirm

    user waits until h3 is visible  Choose a subject

    user clicks link  Featured tables
    user waits until h3 is visible  Choose a table

    user checks element count is x  css:#featuredTables li  1
    user checks element should contain  css:#featuredTables li:first-child a  Test highlight name
    user checks element should contain  css:#featuredTables li:first-child [id^="highlight-description"]
    ...  Test highlight description

    user clicks link  Test highlight name
    user waits until results table appears  180
    user waits until page contains element   xpath://*[@data-testid="dataTableCaption" and text()="Table showing Admission Numbers for '${SUBJECT_NAME}' from '${PUBLICATION_NAME}' in Bolton 001 (E02000984), Bolton 001 (E05000364), Bolton 004 (E02000987), Bolton 004 (E05010450), Nailsea Youngwood and Syon between 2005 and 2020"]

Validate table column headings for table highlight
    [Tags]  HappyPath
    user checks table column heading contains  1  1  Admission Numbers

Validate table rows for table highlight
    [Tags]  HappyPath
    ${row}=  user gets row number with heading  Bolton 001 (E02000984)
    user checks table heading in offset row contains  ${row}  0  2  2019

    user checks table cell in offset row contains  ${row}  0  1  8,533

    ${row}=  user gets row number with heading   Bolton 001 (E05000364)
    user checks table heading in offset row contains  ${row}  0  2  2009
    user checks table heading in offset row contains  ${row}  1  1  2010
    user checks table heading in offset row contains  ${row}  2  1  2017

    user checks table cell in offset row contains  ${row}  0  1  5,815
    user checks table cell in offset row contains  ${row}  1  1  5,595
    user checks table cell in offset row contains  ${row}  2  1  6,373

    ${row}=  user gets row number with heading   Bolton 004 (E02000987)
    user checks table heading in offset row contains  ${row}  0  2  2020

    user checks table cell in offset row contains  ${row}  0  1  6,031

    ${row}=  user gets row number with heading   Bolton 004 (E05010450)
    user checks table heading in offset row contains  ${row}  0  2  2005
    user checks table heading in offset row contains  ${row}  1  1  2017
    user checks table heading in offset row contains  ${row}  2  1  2018

    user checks table cell in offset row contains  ${row}  0  1  8,557
    user checks table cell in offset row contains  ${row}  1  1  3,481
    user checks table cell in offset row contains  ${row}  2  1  8,630

    ${row}=  user gets row number with heading   Nailsea Youngwood
    user checks table heading in offset row contains  ${row}  0  2  2005
    user checks table heading in offset row contains  ${row}  1  1  2010
    user checks table heading in offset row contains  ${row}  2  1  2011
    user checks table heading in offset row contains  ${row}  3  1  2012
    user checks table heading in offset row contains  ${row}  4  1  2016

    user checks table cell in offset row contains  ${row}  0  1  3,612
    user checks table cell in offset row contains  ${row}  1  1  9,304
    user checks table cell in offset row contains  ${row}  2  1  9,603
    user checks table cell in offset row contains  ${row}  3  1  8,150
    user checks table cell in offset row contains  ${row}  4  1  4,198

    ${row}=  user gets row number with heading   Syon
    user checks table heading in offset row contains  ${row}  0  2  2007
    user checks table heading in offset row contains  ${row}  1  1  2008
    user checks table heading in offset row contains  ${row}  2  1  2010
    user checks table heading in offset row contains  ${row}  3  1  2012
    user checks table heading in offset row contains  ${row}  4  1  2017

    user checks table cell in offset row contains  ${row}  0  1  9,914
    user checks table cell in offset row contains  ${row}  1  1  5,505
    user checks table cell in offset row contains  ${row}  2  1  6,060
    user checks table cell in offset row contains  ${row}  3  1  1,109
    user checks table cell in offset row contains  ${row}  4  1  1,959
