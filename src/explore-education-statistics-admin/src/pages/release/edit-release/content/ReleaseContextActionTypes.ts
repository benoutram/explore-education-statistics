import {
  EditableBlock,
  EditableContentBlock,
} from '@admin/services/publicationService';
import { ContentSection, Release } from '@common/services/publicationService';
import { ReleaseContextState } from './ReleaseContext';

export type ContentSectionKeys = keyof Pick<
  Release<EditableContentBlock>,
  | 'summarySection'
  | 'keyStatisticsSection'
  | 'keyStatisticsSecondarySection'
  | 'headlinesSection'
  | 'content'
>;

type BlockMeta = {
  sectionId: string;
  blockId: string;
  sectionKey: ContentSectionKeys;
};

type SectionMeta = Omit<BlockMeta, 'blockId'>;

type ClearState = { type: 'CLEAR_STATE' };

type SetState = { type: 'SET_STATE'; payload: ReleaseContextState };

type SetAvailableDatablocks = {
  type: 'SET_AVAILABLE_DATABLOCKS';
  payload: Pick<ReleaseContextState, 'availableDataBlocks'>;
};

export type RemoveBlockFromSection = {
  type: 'REMOVE_BLOCK_FROM_SECTION';
  payload: {
    meta: BlockMeta;
  };
};

export type UpdateBlockFromSection = {
  type: 'UPDATE_BLOCK_FROM_SECTION';
  payload: {
    block: EditableBlock;
    meta: BlockMeta;
  };
};

export type AddBlockToSection = {
  type: 'ADD_BLOCK_TO_SECTION';
  payload: {
    block: EditableBlock;
    meta: SectionMeta;
  };
};

export type UpdateSectionContent = {
  type: 'UPDATE_SECTION_CONTENT';
  payload: {
    sectionContent: EditableBlock[];
    meta: SectionMeta;
  };
};

export type AddContentSection = {
  type: 'ADD_CONTENT_SECTION';
  payload: {
    section: ContentSection<EditableBlock>;
  };
};

export type SetReleaseContent = {
  type: 'SET_CONTENT';
  payload: {
    content: ContentSection<EditableBlock>[];
  };
};

export type UpdateContentSection = {
  type: 'UPDATE_CONTENT_SECTION';
  payload: {
    meta: { sectionId: string };
    section: ContentSection<EditableBlock>;
  };
};

export type ReleaseDispatchAction =
  | ClearState
  | SetState
  | SetAvailableDatablocks
  | RemoveBlockFromSection
  | UpdateBlockFromSection
  | AddBlockToSection
  | UpdateSectionContent
  | AddContentSection
  | SetReleaseContent
  | UpdateContentSection;
