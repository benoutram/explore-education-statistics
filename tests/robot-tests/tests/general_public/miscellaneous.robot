*** Settings ***
Resource    ../libs/public-common.robot

Force Tags  GeneralPublic  Local  Dev  Test  Preprod  Prod

Suite Setup       user opens the browser
Suite Teardown    user closes the browser

*** Test Cases ***
Verify public page loads
    [Tags]  HappyPath
    environment variable should be set  PUBLIC_URL
    user goes to url  %{PUBLIC_URL}
    user waits until element contains  css:body   Explore education statistics

Verify can accept cookie banner
    [Tags]  HappyPath   NotAgainstLocal
    user checks page contains  We use some essential cookies to make this service work.

    cookie should not exist   ees_banner_seen
    cookie should not exist   ees_disable_google_analytics

    user clicks element  xpath://button[text()="Accept analytics cookies"]

    cookie should have value  ees_banner_seen   true
    cookie should have value  ees_disable_google_analytics   false
    user clicks button  Hide this message

Validate homepage
    [Tags]  HappyPath
    user checks page contains element  link:Explore
    user checks page contains element  link:Create

    user waits until h2 is visible  Supporting information
    user checks page contains element  link:Methodology
    user checks page contains element  link:Glossary
    user waits until h2 is visible  Related services
    user checks page contains element  link:Find and compare schools in England
    user checks page contains element  link:Get information about schools
    user checks page contains element  link:Schools financial benchmarking


    user checks page contains element  xpath://h2[text()="Contact us"]

    user checks page contains link with text and url  explore.statistics@education.gov.uk   mailto:explore.statistics@education.gov.uk

    user checks breadcrumb count should be  1
    user checks nth breadcrumb contains     1   Home

    user checks page contains link with text and url   Open Government Licence v3.0  https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/
    user checks page contains link with text and url   © Crown copyright   https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/

Validate Cookies page
    [Tags]  HappyPath
    user clicks link   Cookies

    user waits until h1 is visible   Cookies on Explore education statistics
    user checks url contains   %{PUBLIC_URL}/cookies

    user checks breadcrumb count should be  2
    user checks nth breadcrumb contains  1   Home
    user checks nth breadcrumb contains  2   Cookies

Disable google analytics
    [Tags]  HappyPath   NotAgainstLocal
    # NOTE: think there is a problem with the frontend. You'd think cookieSettingsForm would only be in the id once...
    user clicks element   id:cookieSettingsForm-cookieSettingsForm-googleAnalytics-off
    user clicks element   xpath://button[text()="Save changes"]
    user waits until page contains   Your cookie settings were saved

    cookie should have value  ees_banner_seen   true
    cookie should have value  ees_disable_google_analytics   true

Enable google analytics
    [Tags]  HappyPath    NotAgainstLocal
    user reloads page
    user checks page does not contain  Your cookie settings were saved
    user waits until h1 is visible   Cookies on Explore education statistics

    sleep  1   # NOTE(mark): Without the wait, the click doesn't select the radio despite the DOM being loaded
    user clicks element   id:cookieSettingsForm-cookieSettingsForm-googleAnalytics-on
    user clicks element   xpath://button[text()="Save changes"]
    user waits until page contains   Your cookie settings were saved
    cookie should have value  ees_banner_seen   true
    cookie should have value  ees_disable_google_analytics   false

Validate Cookies Details page
    [Tags]  HappyPath
    user clicks link   Find out more about cookies on Explore education statistics
    user waits until h1 is visible   Details about cookies
    user checks url contains   %{PUBLIC_URL}/cookies/details

    user checks breadcrumb count should be  3
    user checks nth breadcrumb contains  1   Home
    user checks nth breadcrumb contains  2   Cookies
    user checks nth breadcrumb contains  3   Details about cookies

    cookie names should be on page

Validate Privacy notice page
    [Tags]  HappyPath
    user clicks link   Privacy notice
    user waits until h1 is visible  Privacy notice
    user waits until page contains  The Explore education statistics service is operated by the Department for Education

    user checks url contains  %{PUBLIC_URL}/privacy-notice

    user checks breadcrumb count should be  2
    user checks nth breadcrumb contains  1   Home
    user checks nth breadcrumb contains  2   Privacy notice

Validate Contact page
    [Tags]  HappyPath
    user clicks link    Contact us
    user waits until page contains  Contact Explore education statistics
    user waits until page contains  General enquiries
    user waits until page contains  explore.statistics@education.gov.uk
    user waits until page contains  DfE Head of Profession for Statistics
    user waits until page contains  hop.statistics@education.gov.uk

    user checks url contains    %{PUBLIC_URL}/contact-us

    user checks breadcrumb count should be  2
    user checks nth breadcrumb contains  1   Home
    user checks nth breadcrumb contains  2   Contact

Validate Accessibility statement page
    [Tags]  HappyPath
    user clicks link  Accessibility statement
    user waits until h1 is visible  Accessibility statement for Explore education statistics
    user waits until h2 is visible  What we’re doing to improve accessibility

    user checks url contains  %{PUBLIC_URL}/accessibility-statement

    user checks breadcrumb count should be  2
    user checks nth breadcrumb contains  1   Home
    user checks nth breadcrumb contains  2   Accessibility statement

Validate Help and support page
    [Tags]  HappyPath
    user clicks link    Help and support
    user waits until h1 is visible  Help and support

    user checks url contains    %{PUBLIC_URL}/help-support

    user checks breadcrumb count should be  2
    user checks nth breadcrumb contains  1   Home
    user checks nth breadcrumb contains  2   Help and support

Validate Feedback page
    [Documentation]  EES-942
    [Tags]  HappyPath
    user clicks link  feedback
    user selects newly opened window
    user waits until page contains element      xpath://span[text()="Explore Education Statistics"]     120
    user waits until page contains element      xpath://span[text()="Beta Feedback Survey"]

    user checks url contains    forms.office.com/Pages/ResponsePage.aspx
