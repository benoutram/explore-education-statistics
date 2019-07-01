import useMounted from '@common/hooks/useMounted';
import isComponentType from '@common/lib/type-guards/components/isComponentType';
import classNames from 'classnames';
import React, {
  cloneElement,
  ReactComponentElement,
  ReactNode,
  useEffect,
  useRef,
  useState,
} from 'react';
import { useImmer } from 'use-immer';
import styles from './Accordion.module.scss';
import AccordionSection, {
  accordionSectionClasses,
  AccordionSectionProps,
} from './AccordionSection';

export interface AccordionProps {
  children: ReactNode;
  id: string;
  onToggleAll?: (open: boolean) => void;
}

const Accordion = ({ children, id, onToggleAll }: AccordionProps) => {
  const ref = useRef<HTMLDivElement>(null);

  const [hashId, setHashId] = useState('');
  const [openSections, updateOpenSections] = useImmer<boolean[]>([]);

  const { isMounted } = useMounted(() => {
    const goToHash = () => {
      if (ref.current && window.location.hash) {
        let locationHashEl: HTMLElement | null = null;

        try {
          locationHashEl = ref.current.querySelector(window.location.hash);
        } catch (_) {
          return;
        }

        if (locationHashEl) {
          const sectionEl = locationHashEl.closest(
            `.${accordionSectionClasses.section}`,
          );

          if (sectionEl) {
            const contentEl = sectionEl.querySelector(
              `.${accordionSectionClasses.sectionContent}`,
            );

            if (contentEl) {
              setHashId(contentEl.id);

              (locationHashEl as HTMLElement).scrollIntoView({
                block: 'start',
              });
            }
          }
        }
      }
    };

    goToHash();
    window.addEventListener('hashchange', goToHash);
  });

  const sections = React.Children.toArray(children).filter(child =>
    isComponentType(child, AccordionSection),
  ) as ReactComponentElement<typeof AccordionSection>[];

  useEffect(() => {
    updateOpenSections(() =>
      sections.map(section => section.props.open || false),
    );
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [children, updateOpenSections]);

  const isAllOpen = openSections.every(isOpen => isOpen);

  return (
    <div
      className={classNames('govuk-accordion', styles.accordionPrint)}
      id={id}
      ref={ref}
      role="none"
    >
      {isMounted && (
        <div className="govuk-accordion__controls">
          <button
            aria-expanded={isAllOpen}
            type="button"
            className="govuk-accordion__open-all"
            onClick={() => {
              updateOpenSections(draft => {
                return draft.map(() => !isAllOpen);
              });

              if (onToggleAll) {
                onToggleAll(!isAllOpen);
              }
            }}
          >
            {isAllOpen ? 'Close all ' : 'Open all '}
            <span className="govuk-visually-hidden">sections</span>
          </button>
        </div>
      )}

      {sections.map((section, index) => {
        const contentId =
          section.props.contentId || `${id}-${index + 1}-content`;
        const headingId =
          section.props.headingId || `${id}-${index + 1}-heading`;

        const isSectionOpen =
          isAllOpen ||
          openSections[index] ||
          hashId === contentId ||
          hashId === headingId;

        return cloneElement<AccordionSectionProps>(section, {
          key: headingId,
          contentId,
          headingId,
          open: isSectionOpen,
          onToggle(isOpen) {
            updateOpenSections(draft => {
              draft[index] = isOpen;
            });

            if (section.props.onToggle) {
              section.props.onToggle(isOpen);
            }
          },
        });
      })}
    </div>
  );
};

export default Accordion;
