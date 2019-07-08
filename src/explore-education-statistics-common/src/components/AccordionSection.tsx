import useMounted from '@common/hooks/useMounted';
import findAllParents from '@common/lib/dom/findAllParents';
import printStyles from '@frontend/components/PrintCSS.module.scss';
import classNames from 'classnames';
import React, { createElement, ReactNode } from 'react';
import GoToTopLink from './GoToTopLink';

export type ToggleHandler = (open: boolean) => void;

export interface AccordionSectionProps {
  caption?: string;
  children: ReactNode;
  className?: string;
  contentId?: string;
  goToTop?: boolean;
  heading: string;
  headingId?: string;
  /**
   * Only for accessibility/semantic markup,
   * does not change the actual styling
   */
  headingTag?: 'h2' | 'h3' | 'h4';
  id?: string;
  open?: boolean;
  onToggle?: ToggleHandler;
}

const classes = {
  section: 'govuk-accordion__section',
  sectionButton: 'govuk-accordion__section-button',
  sectionContent: 'govuk-accordion__section-content',
  sectionHeading: 'govuk-accordion__section-heading',
  expanded: 'govuk-accordion__section--expanded',
};

export const accordionSectionClasses = classes;

const AccordionSection = ({
  caption,
  className,
  children,
  contentId,
  goToTop = true,
  heading,
  headingId,
  headingTag = 'h2',
  open = false,
  onToggle,
}: AccordionSectionProps) => {
  const { isMounted } = useMounted();

  return (
    <div
      className={classNames(classes.section, className, {
        [classes.expanded]: open,
      })}
      role="presentation"
    >
      <div
        className={classNames(
          printStyles.dontBreakAfter,
          'govuk-accordion__section-header',
        )}
      >
        {createElement(
          headingTag,
          {
            className: classes.sectionHeading,
          },
          isMounted ? (
            <>
              <button
                aria-expanded={open}
                className={classes.sectionButton}
                id={headingId}
                type="button"
                onClick={() => {
                  if (onToggle) {
                    onToggle(!open);
                  }
                }}
              >
                {heading}
                <span aria-hidden className="govuk-accordion__icon" />
              </button>
            </>
          ) : (
            <span id={headingId}>{heading}</span>
          ),
        )}
        {caption && (
          <span className="govuk-accordion__section-summary">{caption}</span>
        )}
      </div>

      <div
        className={classes.sectionContent}
        aria-labelledby={headingId}
        id={contentId}
      >
        {children}

        {goToTop && <GoToTopLink />}
      </div>
    </div>
  );
};

export default AccordionSection;

export const openAllParentAccordionSections = (target: HTMLElement) => {
  const sections = findAllParents(target, `.${classes.section}`);

  sections.forEach(section => {
    const button = section.querySelector<HTMLElement>(
      `.${classes.sectionButton}`,
    );

    if (button && button.getAttribute('aria-expanded') === 'false') {
      button.click();
    }
  });
};
