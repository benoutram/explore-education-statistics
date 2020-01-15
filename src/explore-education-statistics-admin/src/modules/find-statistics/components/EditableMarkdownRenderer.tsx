import { ErrorControlContext } from '@admin/components/ErrorBoundary';
import WysiwygEditor from '@admin/components/WysiwygEditor';
import { RendererProps } from '@admin/modules/find-statistics/PublicationReleaseContent';
import React, { useContext } from 'react';
import ReactMarkdown, { ReactMarkdownProps } from 'react-markdown';
import wrapEditableComponent from '@common/modules/find-statistics/util/wrapEditableComponent';
import { EditingContentBlockContext } from '@admin/modules/find-statistics/components/EditableContentBlocks';
import { releaseContentService } from '@admin/services/release/edit-release/content/service';

export type MarkdownRendererProps = RendererProps &
  ReactMarkdownProps & {
    canDelete: boolean;
    onDelete: () => void;
    editable?: boolean;
    onContentChange?: (content: string) => void;
  };

const EditableMarkdownRenderer = ({
  contentId,
  source,
  canDelete,
  onDelete,
  editable = true,
  onContentChange,
}: MarkdownRendererProps) => {
  const [markdown, setMarkdown] = React.useState(source);

  const editingContext = React.useContext(EditingContentBlockContext);

  const { handleApiErrors } = useContext(ErrorControlContext);

  return (
    <>
      <WysiwygEditor
        content={markdown || ''}
        canDelete={canDelete}
        editable={editable}
        useMarkdown
        onContentChange={async ss => {
          if (
            editingContext.releaseId &&
            editingContext.sectionId &&
            contentId
          ) {
            const { body } = await releaseContentService
              .updateContentSectionBlock(
                editingContext.releaseId,
                editingContext.sectionId,
                contentId,
                {
                  body: ss,
                },
              )
              .catch(handleApiErrors);

            if (onContentChange) onContentChange(body);
            setMarkdown(body);
          }
        }}
        onDelete={onDelete}
      />
    </>
  );
};

export default wrapEditableComponent(EditableMarkdownRenderer, ReactMarkdown);
