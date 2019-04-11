import React, { cloneElement, Component, createRef, ReactNode } from 'react';
import PrototypeEditableAccordionSection, {AccordionSectionProps} from "./PrototypeEditableAccordionSection";
import isComponentType from "../../lib/type-guards/components/isComponentType";

export interface AccordionProps {
  children: ReactNode;
  id: string;
}

interface State {
  hash: string;
}

class PrototypeAccordion extends Component<AccordionProps, State> {
  public state = {
    hash: '',
  };

  private ref = createRef<HTMLDivElement>();

  public componentDidMount(): void {
    import('govuk-frontend/components/accordion/accordion').then(
      ({ default: GovUkAccordion }) => {
        if (this.ref.current) {
          new GovUkAccordion(this.ref.current).init();
        }
      },
    );

    this.goToHash();
    window.addEventListener('hashchange', this.goToHash);
  }

  public componentWillUnmount(): void {
    window.removeEventListener('hashchange', this.goToHash);
  }

  private goToHash = () => {
    this.setState({ hash: location.hash });

    if (this.ref.current && location.hash) {
      const anchor = this.ref.current.querySelector(
        location.hash,
      ) as HTMLButtonElement;

      if (anchor) {
        anchor.scrollIntoView();
      }
    }
  };

  public render() {
    const { children, id } = this.props;
    const { hash } = this.state;

    let sectionId = 0;

    return (
      <div className="govuk-accordion" ref={this.ref} id={id}>
        {React.Children.map(children, child => {
          if (isComponentType(child, PrototypeEditableAccordionSection)) {
            sectionId += 1;

            const contentId = `${id}-content-${sectionId}`;
            const headingId = `${id}-heading-${sectionId}`;

            const isLocationHashMatching =
              hash === `#${headingId}` || hash === `#${contentId}`;

            return cloneElement<AccordionSectionProps>(child, {
              contentId,
              headingId,
              index: sectionId-1,
              open: child.props.open || isLocationHashMatching,
            });
          }

          return child;
        })}
      </div>
    );
  }
}

export default PrototypeAccordion;
