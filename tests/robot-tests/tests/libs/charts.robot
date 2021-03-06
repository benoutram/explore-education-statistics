*** Settings ***
Resource    ./common.robot

*** Keywords ***
user waits for chart preview to update
    # We check that the chart has updated using the number of renders as
    # there is no other good way to check that the DOM has actually changed.
    ${count}=  get element attribute  id:chartBuilderPreviewContainer  data-render-count
    ${next_count}=  evaluate  int(${count}) + 1
    user waits until page contains element  css:#chartBuilderPreviewContainer[data-render-count="${next_count}"]

user waits until element contains line chart
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:.recharts-line

user waits until element does not contain line chart
    [Arguments]  ${locator}
    user waits until parent does not contain element  ${locator}  css:.recharts-line

user waits until element contains bar chart
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:.recharts-bar-rectangles

user waits until element does not contain bar chart
    [Arguments]  ${locator}
    user waits until parent does not contain element  ${locator}  css:.recharts-bar-rectangles

user waits until element contains map chart
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:.leaflet-pane

user waits until element does not contain map chart
    [Arguments]  ${locator}
    user waits until parent does not contain element  ${locator}  css:.leaflet-pane

user waits until element contains infographic chart
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:img

user waits until element does not contain infographic chart
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:img

user waits until element contains chart tooltip
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:[data-testid="chartTooltip"]

user waits until element does not contain chart tooltip
    [Arguments]  ${locator}
    user waits until parent does not contain element  ${locator}  css:[data-testid="chartTooltip"]

user checks chart title contains
    [Arguments]  ${locator}  ${text}
    user waits until parent contains element  ${locator}  xpath://figcaption[text()="${text}"]

user checks chart height
    [Arguments]  ${locator}  ${height}
    user waits until parent contains element  ${locator}  css:.recharts-surface[height="${height}"]

user checks chart width
    [Arguments]  ${locator}  ${width}
    user waits until parent contains element  ${locator}  css:.recharts-surface[width="${width}"]

user checks map chart height
    [Arguments]  ${locator}  ${height}  ${unit}=px
    user waits until parent contains element  ${locator}  css:.leaflet-container
    ${element}=  get child element  ${locator}  css:.leaflet-container
    user checks css property value  ${element}  height  ${height}${unit}

user checks map chart width
    [Arguments]  ${locator}  ${width}  ${unit}=px
    user waits until parent contains element  ${locator}  css:.leaflet-container
    ${element}=  get child element  ${locator}  css:.leaflet-container
    user checks css property value  ${element}  width  ${width}${unit}

user checks chart legend item contains
    [Arguments]  ${locator}  ${item}  ${text}
    user waits until parent contains element  ${locator}  css:.recharts-default-legend li:nth-of-type(${item})
    ${element}=  get child element  ${locator}  css:.recharts-default-legend li:nth-of-type(${item})
    user waits until element is visible  ${element}
    user waits until element contains  ${element}  ${text}

user checks chart y axis tick contains
    [Arguments]  ${locator}  ${tick}  ${text}
    user waits until parent contains element  ${locator}  css:.recharts-yAxis .recharts-cartesian-axis-tick:nth-of-type(${tick})
    ${element}=  get child element  ${locator}  css:.recharts-yAxis .recharts-cartesian-axis-tick:nth-of-type(${tick})
    user waits until element is visible  ${element}
    user waits until element contains  ${element}  ${text}

user checks chart x axis tick contains
    [Arguments]  ${locator}  ${tick}  ${text}
    user waits until parent contains element  ${locator}  css:.recharts-xAxis .recharts-cartesian-axis-tick:nth-of-type(${tick})
    ${element}=  get child element  ${locator}  css:.recharts-xAxis .recharts-cartesian-axis-tick:nth-of-type(${tick})
    user waits until element is visible  ${element}
    user waits until element contains  ${element}  ${text}

user checks chart y axis ticks
    [Arguments]  ${locator}  @{ticks}
    FOR  ${index}  ${tick}  IN ENUMERATE  @{ticks}
        user checks chart y axis tick contains  ${locator}  ${index + 1}  ${tick}
    END

user checks chart x axis ticks
    [Arguments]  ${locator}  @{ticks}
    FOR  ${index}  ${tick}  IN ENUMERATE  @{ticks}
        user checks chart x axis tick contains  ${locator}  ${index + 1}  ${tick}
    END

user mouses over line chart point
    [Arguments]  ${locator}  ${line}  ${number}
    user waits until parent contains element  ${locator}  css:.recharts-line-dots:nth-of-type(${line}) .recharts-symbols:nth-of-type(${number})
    ${element}=  get child element  ${locator}  css:.recharts-line-dots:nth-of-type(${line}) .recharts-symbols:nth-of-type(${number})
    user waits until element is visible  ${element}
    user mouses over element  ${element}

user mouses over chart bar
    [Arguments]  ${locator}  ${number}
    user waits until parent contains element  ${locator}  css:.recharts-bar-rectangles .recharts-bar-rectangle:nth-of-type(${number})
    ${element}=  get child element  ${locator}  css:.recharts-bar-rectangles .recharts-bar-rectangle:nth-of-type(${number})
    user waits until element is visible  ${element}
    user mouses over element  ${element}

user mouses over selected map feature
    [Arguments]  ${locator}
    user waits until parent contains element  ${locator}  css:[data-testid="mapBlock-selectedFeature"]
    ${element}=  get child element  ${locator}  css:[data-testid="mapBlock-selectedFeature"]
    user waits until element is visible  ${element}
    user mouses over element  ${element}

user checks chart tooltip label contains
    [Arguments]  ${locator}  ${text}
    user waits until parent contains element  ${locator}  css:[data-testid="chartTooltip-label"]
    ${element}=  get child element  ${locator}  css:[data-testid="chartTooltip-label"]
    user waits until element is visible  ${element}
    user waits until element contains  ${element}  ${text}

user checks chart tooltip item contains
    [Arguments]  ${locator}  ${item}  ${text}
    user waits until parent contains element  ${locator}  css:[data-testid="chartTooltip-items"] li:nth-of-type(${item})
    ${element}=  get child element  ${locator}  css:[data-testid="chartTooltip-items"] li:nth-of-type(${item})
    user waits until element is visible  ${element}
    user waits until element contains  ${element}  ${text}

user checks map chart indicator tile contains
    [Arguments]  ${locator}  ${number}  ${title}  ${value}
    user waits until parent contains element  ${locator}  css:[data-testid="mapBlock-indicator"]:nth-of-type(${number})
    ${indicator}=  get child element  ${locator}  css:[data-testid="mapBlock-indicator"]:nth-of-type(${number})

    ${indicator_title}=  get child element  ${indicator}  css:[data-testid="mapBlock-indicatorTile-title"]
    user waits until element is visible  ${indicator_title}
    user waits until element contains  ${indicator_title}  ${title}

    ${indicator_value}=  get child element  ${indicator}  css:[data-testid="mapBlock-indicatorTile-value"]
    user waits until element is visible  ${indicator_value}
    user waits until element contains  ${indicator_value}  ${value}

user checks infographic chart contains alt
    [Arguments]  ${locator}  ${text}
    user waits until parent contains element  ${locator}  css:img[alt="${text}"]
