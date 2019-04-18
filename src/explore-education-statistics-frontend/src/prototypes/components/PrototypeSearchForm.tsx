import debounce from 'lodash/debounce';
import React, { Component, FormEvent, KeyboardEvent } from 'react';
import styles from './PrototypeSearchForm.module.scss';
import _ from 'lodash';

interface SearchResult {
  element: Element;
  scrollIntoView: () => void;
  text: string;
  location: string[];
}

interface State {
  currentlyHighlighted?: number;
  searchResults: SearchResult[];
  searchValue: string;
}

class PrototypeSearchForm extends Component<{}, State> {
  public state: State = {
    searchResults: [],
    searchValue: '',
  };
  private readonly boundPerformSearch: () => void;

  private findElementsWithText(text: string) {
    const lowerCase = text.toLocaleLowerCase();
    return Array.from(document.querySelectorAll('p')).filter(e =>
      e.innerHTML.toLocaleLowerCase().includes(lowerCase),
    );
  }

  private static parentUntilClassname(
    element: Element,
    ...className: string[]
  ) {
    let parentElement = element.parentElement;
    while (
      parentElement &&
      parentElement !== document.documentElement &&
      _.intersection(className, parentElement.classList).length === 0
    ) {
      parentElement = parentElement.parentElement;
    }
    return parentElement || document.documentElement;
  }

  private static findSiblingsBeforeOfElementType(
    element: Element,
    ...types: string[]
  ) {
    let sibling = element.previousElementSibling;

    const typesUpper = types.map(_ => _.toUpperCase());

    while (sibling && !typesUpper.includes(sibling.nodeName)) {
      sibling = sibling.previousElementSibling;
    }

    return sibling;
  }

  private static substring(str: string, length: number) {
    if (str) {
      if (str.length > length) {
        return `${str.substring(0, length - 1)}…`;
      }
      return str;
    }
    return '';
  }

  public constructor(props: {}) {
    super(props);

    this.boundPerformSearch = debounce(this.performSearch, 1000);
  }

  private performSearch() {
    if (this.state.searchValue.length <= 3) {
      return;
    }

    const elements = this.findElementsWithText(this.state.searchValue);

    const searchResults: SearchResult[] = elements.map(element => {
      let scrollIntoView: () => void;

      const collapsedContainer = PrototypeSearchForm.parentUntilClassname(
        element,
        'govuk-accordion__section',
        'govuk-details',
      );

      const location = PrototypeSearchForm.calculateLocationOfElement(
        element,
        collapsedContainer,
      );

      if (collapsedContainer.classList.contains('govuk-accordion__section')) {
        scrollIntoView = () => {
          PrototypeSearchForm.openAccordion(collapsedContainer);
          this.resetSearch();
          element.scrollIntoView();
        };
      } else if (collapsedContainer.classList.contains('govuk-details')) {
        scrollIntoView = () => {
          collapsedContainer.setAttribute('open', 'open');
          this.resetSearch();
          element.scrollIntoView();
        };
      } else {
        scrollIntoView = () => {
          this.resetSearch();
          element.scrollIntoView();
        };
      }

      return {
        element,
        location,
        scrollIntoView,
        text: PrototypeSearchForm.substring(element.textContent || '', 30),
      };
    });

    this.setState({ searchResults });
  }

  private static openAccordion(potentialAccordion: HTMLElement) {
    potentialAccordion.classList.add('govuk-accordion__section--expanded');
  }

  private resetSearch() {
    this.setState({
      currentlyHighlighted: undefined,
      searchResults: [],
      searchValue: '',
    });
  }

  private static calculateLocationOfElement(
    element: HTMLElement,
    collapsedContainer: HTMLElement,
  ) {
    const location: string[] = [];

    const locationHeaderElement = PrototypeSearchForm.findSiblingsBeforeOfElementType(
      element,
      'h3',
      'h2',
      'h4',
    );

    if (locationHeaderElement) {
      location.unshift(
        PrototypeSearchForm.substring(
          locationHeaderElement.textContent || '',
          20,
        ),
      );
    }

    if (collapsedContainer.classList.contains('govuk-accordion__section')) {
      const accordionHeader = collapsedContainer.querySelector(
        '.govuk-accordion__section-heading button',
      );
      if (accordionHeader) {
        location.unshift(
          PrototypeSearchForm.substring(accordionHeader.textContent || '', 20),
        );
      }
    }

    return location;
  }

  private onKeyDown = (e: KeyboardEvent) => {
    if (e.key === 'ArrowUp' || e.key === 'ArrowDown') {
      const direction = e.key === 'ArrowUp' ? -1 : 1;

      const len = this.state.searchResults.length;

      let currentlyHighlighted: number | undefined = this.state
        .currentlyHighlighted;

      if (currentlyHighlighted !== undefined) {
        currentlyHighlighted =
          ((this.state.currentlyHighlighted || 0) + direction + len) % len;
      } else {
        if (direction === -1) {
          currentlyHighlighted = len - 1;
        } else {
          currentlyHighlighted = 0;
        }
      }

      this.setState({ currentlyHighlighted });
    }

    if (e.key === 'Enter') {
      if (this.state.currentlyHighlighted !== undefined) {
        this.state.searchResults[
          this.state.currentlyHighlighted
        ].scrollIntoView();

        e.preventDefault();
      }
    }
  };

  private onChange = (e: FormEvent<HTMLInputElement>) => {
    e.persist();

    this.setState({
      searchValue: e.currentTarget.value,
      searchResults: [],
      currentlyHighlighted: undefined,
    });

    this.boundPerformSearch();
  };

  public render() {
    return (
      <form
        className={styles.container}
        onSubmit={e => e.preventDefault()}
        autoComplete="off"
        role="search"
      >
        <div className="govuk-form-group govuk-!-margin-bottom-0">
          <label className="govuk-label govuk-visually-hidden" htmlFor="search">
            Find on this page
          </label>

          <input
            className="govuk-input"
            id="search"
            placeholder="Search this page"
            type="search"
            value={this.state.searchValue}
            onKeyDown={this.onKeyDown}
            onInput={this.onChange}
          />
          <input
            type="submit"
            className={styles.dfeSearchButton}
            value="Search this page"
            onClick={() => this.performSearch()}
          />
        </div>
        {this.state.searchResults.length > 0 ? (
          <ul className={styles.results}>
            {this.state.searchResults.map((result: SearchResult, index) => (
              // eslint-disable-next-line jsx-a11y/no-noninteractive-element-interactions,jsx-a11y/click-events-have-key-events
              <li
                key={`search_result_${index}`}
                className={
                  this.state.currentlyHighlighted === index
                    ? styles.highlighted
                    : ''
                }
                onClick={result.scrollIntoView}
              >
                <span className={styles.resultHeader}>{result.text}</span>
                <span className={styles.resultLocation}>
                  {result.location.join(' > ')}
                </span>
              </li>
            ))}
          </ul>
        ) : (
          ''
        )}
      </form>
    );
  }
}

export default PrototypeSearchForm;
