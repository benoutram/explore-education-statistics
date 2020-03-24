DROP TABLE __EFMigrationsHistory;
DROP TABLE ObservationFilterItem;
DROP TABLE FilterItemFootnote;
DROP TABLE FilterGroupFootnote;
DROP TABLE FilterFootnote;
DROP TABLE FilterItem;
DROP TABLE FilterGroup;
DROP TABLE Filter;
DROP TABLE IndicatorFootnote;
DROP TABLE SubjectFootnote;
DROP TABLE Footnote;
DROP TABLE Indicator;
DROP TABLE IndicatorGroup;
DROP TABLE Observation;
DROP TABLE School;
DROP TABLE Provider;
DROP TABLE Location;
DROP TABLE Subject;
DROP TABLE Release;
DROP TABLE Publication;
DROP TABLE Topic;
DROP TABLE Theme;
DROP PROCEDURE FilteredObservations;
DROP PROCEDURE FilteredFootnotes;
DROP PROCEDURE UpsertLocation;
DROP PROCEDURE UpsertPublication;
DROP PROCEDURE UpsertTheme;
DROP PROCEDURE UpsertTopic;
DROP PROCEDURE DropAndCreateRelease;
DROP PROCEDURE InsertObservationFilterItems;
DROP PROCEDURE InsertObservations;
DROP TYPE FootnoteType;
DROP TYPE IdListGuidType;
DROP TYPE IdListIntegerType;
DROP TYPE IdListVarcharType;
DROP TYPE LocationType;
DROP TYPE PublicationType;
DROP TYPE ReleaseType;
DROP TYPE ThemeType;
DROP TYPE TimePeriodListType;
DROP TYPE TopicType;
DROP TYPE ObservationFilterItemType;
DROP TYPE ObservationType;
DROP VIEW geojson
DROP FUNCTION geometry2json;
DROP TABLE geometry;
DROP TABLE BoundaryLevel;
DROP TABLE spatial_ref_sys;
DROP TABLE geometry_columns;