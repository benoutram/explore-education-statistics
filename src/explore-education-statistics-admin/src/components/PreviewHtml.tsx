import ContentHtml from '@common/components/ContentHtml';
import classNames from 'classnames';
import React from 'react';
import styles from './PreviewHtml.module.scss';

interface Props {
  className?: string;
  html: string;
  testId?: string;
}

const PreviewHtml = ({ className, html, testId }: Props) => {
  return (
    <ContentHtml
      html={html}
      className={classNames(styles.preview, className)}
      testId={testId}
    />
  );
};

export default PreviewHtml;
