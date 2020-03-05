import { EditableContentBlock } from '@admin/services/publicationService';
import React, { MouseEventHandler, ReactNode, useCallback } from 'react';
import ContentBlock, {
  ReorderHook,
} from '@admin/modules/find-statistics/components/EditableContentBlocks';
import AccordionSection, {
  EditableAccordionSectionProps,
} from '@admin/components/EditableAccordionSection';
import classnames from 'classnames';
import styles from '@admin/modules/find-statistics/components/ReleaseContentAccordion.module.scss';
import { ContentType } from '@admin/modules/find-statistics/components/ReleaseContentAccordion';
import { AbstractRelease } from '@common/services/publicationService';

export interface ReleaseContentAccordionSectionProps {
  id?: string;
  contentItem: ContentType;
  index: number;
  publication: AbstractRelease<EditableContentBlock>['publication'];
  headingButtons?: ReactNode[];
  onHeadingChange?: EditableAccordionSectionProps['onHeadingChange'];
  onContentChange?: (content?: EditableContentBlock[]) => void;
  canToggle?: boolean;
  onRemoveSection?: EditableAccordionSectionProps['onRemoveSection'];
}

const ReleaseContentAccordionSection = ({
  id,
  index,
  contentItem,
  publication,
  headingButtons,
  canToggle = true,
  onHeadingChange,
  onContentChange,
  onRemoveSection,
  ...restOfProps
}: ReleaseContentAccordionSectionProps) => {
  const { caption, heading } = contentItem;

  const [isReordering, setIsReordering] = React.useState(false);

  const [content, setContent] = React.useState(contentItem.content);

  const saveOrder = React.useRef<ReorderHook>();

  const onReorderClick: MouseEventHandler = e => {
    e.stopPropagation();
    e.preventDefault();

    if (isReordering) {
      if (saveOrder.current) {
        saveOrder.current(contentItem.id).then(() => setIsReordering(false));
      } else {
        setIsReordering(false);
      }
    } else {
      setIsReordering(true);
    }
  };

  const handleContentChange = React.useCallback(
    (newContent?: EditableContentBlock[]) => {
      setContent(newContent);
      if (onContentChange) {
        onContentChange(newContent);
      }
    },
    [onContentChange],
  );

  const handleReorder = useCallback(ord => {
    saveOrder.current = ord;
  }, []);

  return (
    <AccordionSection
      id={id}
      index={index}
      heading={heading || ''}
      caption={caption}
      canToggle={canToggle}
      headingButtons={[
        content && content.length > 1 && (
          <button
            key="toggle_reordering"
            className={classnames(styles.toggleContentDragging)}
            type="button"
            onClick={onReorderClick}
          >
            {isReordering ? 'Save order' : 'Reorder Content'}
          </button>
        ),
        ...(headingButtons || []),
      ]}
      canEditHeading
      onHeadingChange={onHeadingChange}
      onRemoveSection={onRemoveSection}
      {...restOfProps}
    >
      <ContentBlock
        id={`${heading}-content`}
        isReordering={isReordering}
        canAddBlocks
        sectionId={id}
        content={content}
        publication={publication}
        onContentChange={handleContentChange}
        onReorder={handleReorder}
      />
    </AccordionSection>
  );
};

export default ReleaseContentAccordionSection;
