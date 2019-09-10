*** Settings ***
Library     SeleniumLibrary  timeout=${timeout}  implicit_wait=${implicit_wait}  run_on_failure=do this on failure
Library     OperatingSystem
#Library     XvfbRobot           # sudo apt install xvfb + pip install robotframework-xvfb

#Library    email_guerrillamail.py
Library    file_operations.py
Library    utilities.py

*** Variables ***
${browser}    chrome
${headless}   1

${timeout}          20
${implicit_wait}    20

*** Keywords ***
do this on failure
  capture page screenshot
  set selenium timeout  3
  set selenium implicit wait  3

user opens the browser
  run keyword if    "${browser}" == "chrome"    user opens chrome
  run keyword if    "${browser}" == "firefox"   user opens firefox
  go to    about:blank

user opens chrome
  run keyword if    ${headless} == 1      user opens chrome headless
  #run keyword if    ${headless} == 1      user opens chrome with xvfb
  run keyword if    ${headless} == 0      user opens chrome without xvfb

user opens firefox
  run keyword if    ${headless} == 1      user opens firefox headless
  #run keyword if    ${headless} == 1      user opens firefox with xvfb
  run keyword if    ${headless} == 0      user opens firefox without xvfb

# Requires chromedriver v2.31+ -- you can alternatively use "user opens chrome with xvfb"
user opens chrome headless
  ${c_opts} =     Evaluate    sys.modules['selenium.webdriver'].ChromeOptions()    sys, selenium.webdriver
  Call Method    ${c_opts}   add_argument    headless
  Call Method    ${c_opts}   add_argument    disable-gpu
  Call Method    ${c_opts}   add_argument    window-size\=1920,1080
  Create Webdriver    Chrome    crm_alias    chrome_options=${c_opts}

user opens chrome with xvfb
  start virtual display   1920    1080
  ${options}=    evaluate    sys.modules['selenium.webdriver'].ChromeOptions()    sys

  # --no-sandbox allows chrome to run in a docker container: https://github.com/jessfraz/dockerfiles/issues/149
  run keyword if    ${docker} == 1     Call Method   ${options}   add_argument  --no-sandbox

  create webdriver    Chrome   chrome_options=${options}
  set window size    1920    1080

user opens chrome without xvfb
  open browser   about:blank   Chrome
  maximize browser window

user opens firefox headless
  ${f_opts} =     Evaluate     sys.modules['selenium.webdriver'].firefox.options.Options()    sys, selenium.webdriver
  Call Method    ${f_opts}   add_argument    -headless
  Create Webdriver    Firefox    firefox_options=${f_opts}
  
user opens firefox with xvfb
  start virtual display   1920    1080
  open browser   about:blank   firefox
  set window size   1920    1080

user opens firefox without xvfb
  open browser   about:blank   firefox
  maximize browser window

user closes the browser
  close browser

user goes to url
  [Arguments]   ${destination}
  go to   ${destination}

user goes back
  go back

user reloads page
  reload page

user scrolls to the top of the page
  execute javascript      window.scrollTo(0, 0);

user waits until page contains
  [Arguments]    ${pageText}
  wait until page contains   ${pageText}

user waits until page contains element
  [Arguments]    ${element}
  wait until page contains element  ${element}

user waits until page does not contain element
  [Arguments]    ${element}
  wait until page does not contain element  ${element}

user waits until element contains
  [Arguments]    ${element}    ${text}
  wait until element contains    ${element}    ${text}

user waits until page contains link
  [Arguments]    ${link_text}
  page should contain link   ${link_text}

user waits until page contains heading
  [Arguments]   ${text}
  wait until page contains element   xpath://h1[text()="${text}"]

user checks element contains
  [Arguments]   ${element}    ${text}
  wait until element contains  ${element}    ${text}
  element should contain    ${element}    ${text}

user checks element does not contain
  [Arguments]   ${element}    ${text}
  element should not contain    ${element}    ${text}

user waits until element is visible
  [Arguments]    ${selector}
  wait until element is visible  ${selector}

user checks element is visible
  [Arguments]   ${element}
  element should be visible   ${element}

user checks element is not visible
  [Arguments]   ${element}
  element should not be visible   ${element}

user checks checkbox is selected
  [Arguments]    ${checkbox}
  checkbox should be selected   ${checkbox}

user checks checkbox is not selected
  [Arguments]    ${checkbox}
  checkbox should not be selected   ${checkbox}

user waits until element is enabled
  [Arguments]   ${element}
  wait until element is enabled   ${element}

user checks element is enabled
  [Arguments]   ${element}
  element should be enabled   ${element}

user checks element is disabled
  [Arguments]   ${element}
  element should be disabled   ${element}

user checks element should contain
  [Arguments]   ${element}  ${text}
  element should contain  ${element}    ${text}

user checks element should not contain
  [Arguments]   ${element}  ${text}
  element should not contain  ${element}    ${text}

user checks page contains
  [Arguments]   ${text}
  page should contain   ${text}

user checks page does not contain
  [Arguments]  ${text}
  page should not contain   ${text}

user checks page contains element
  [Arguments]  ${element}
  page should contain element  ${element}

user checks page does not contain element
  [Arguments]  ${element}
  page should not contain element  ${element}

user clicks element
  [Arguments]     ${elementToClick}
  wait until page contains element  ${elementToClick}
  set focus to element    ${elementToClick}
  click element   ${elementToClick}

user clicks link
  [Arguments]   ${text}
  click element  xpath://a[text()="${text}"]

user clicks button
  [Arguments]   ${text}
  click button  ${text}

user selects newly opened window
  select window   NEW

user checks element attribute value should be
  [Arguments]   ${locator}  ${attribute}    ${expected}
  element attribute value should be  ${locator}     ${attribute}   ${expected}

user selects from list by label
  [Arguments]   ${locator}   ${label}
  select from list by label   ${locator}   ${label}

user presses keys
  [Arguments]   ${keys}
  press keys  ${None}    ${keys}

user enters text into element
  [Arguments]   ${selector}   ${text}
  user clicks element   ${selector}
  user presses keys     ${text}

user checks element count is x
  [Arguments]   ${locator}   ${amount}
  page should contain element   ${locator}   limit=${amount}

user checks url contains
  [Arguments]   ${text}
  ${current_url}=   get location
  should contain  ${current_url}   ${text}

user checks page contains link with text and url
  [Arguments]  ${text}  ${href}
  user checks page contains element  xpath://a[@href="${href}" and text()="${text}"]

user waits until results table appears
  # NOTE(mark): Increasing timeout to stop known failure and created DFE-1158
  set selenium timeout          60
  set selenium implicit wait    60
  user waits until page contains element   css:table thead th
  set selenium timeout          ${timeout}
  set selenium implicit wait    ${implicit_wait}

user logs into microsoft online
  [Arguments]  ${email}   ${password}
  user waits until page contains element  xpath://div[text()="Sign in"]
  sleep  1
  user presses keys     ${email}
  user waits until page contains element    css:input[value="Next"]
  wait until element is enabled   css:input[value="Next"]
  user clicks element   css:input[value="Next"]

  user waits until page contains element  xpath://div[text()="Enter password"]
  sleep  1
  user presses keys     ${password}
  user waits until page contains element    css:input[value="Sign in"]
  wait until element is enabled   css:input[value="Sign in"]
  user clicks element   css:input[value="Sign in"]

  user waits until page contains element  xpath://div[text()="Stay signed in?"]
  user waits until page contains element    css:input[value="No"]
  wait until element is enabled   css:input[value="No"]
  sleep  1
  user clicks element   css:input[value="No"]
