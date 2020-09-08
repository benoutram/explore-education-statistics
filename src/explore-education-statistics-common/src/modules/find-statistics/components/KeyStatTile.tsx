import React, { ReactNode } from 'react';
import styles from './KeyStatTile.module.scss';

interface Props {
  children?: ReactNode;
  testId?: string;
  title: string;
  titleTag?: 'h3' | 'h4';
  value: string;
}

const KeyStatTile = ({
  children,
  testId = 'keyStatTile',
  titleTag: TitleElement = 'h3',
  title,
  value,
}: Props) => {
  return (
    <div className={styles.tile}>
      <TitleElement className="govuk-heading-s" data-testid={`${testId}-title`}>
        {title}
      </TitleElement>

      <p className="govuk-heading-xl" data-testid={`${testId}-value`}>
        {value}
      </p>

      {children}
    </div>
  );
};

export default KeyStatTile;
