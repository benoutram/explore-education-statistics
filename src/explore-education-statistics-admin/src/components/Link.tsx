import classNames from 'classnames';
import React, { ReactNode } from 'react';
import { Link as RouterLink, LinkProps } from 'react-router-dom';

export type Props = {
  children: ReactNode;
  className?: string;
  unvisited?: boolean;
} & LinkProps;

export const Link = ({
  children,
  className,
  to,
  unvisited = false,
  ...props
}: Props) => {
  return (
    <RouterLink
      {...props}
      to={to}
      className={classNames(
        'govuk-link',
        {
          'govuk-link--no-visited-state': unvisited,
        },
        className,
      )}
    >
      {children}
    </RouterLink>
  );
};

export default Link;
