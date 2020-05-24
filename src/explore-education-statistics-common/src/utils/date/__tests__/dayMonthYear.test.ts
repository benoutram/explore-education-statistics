import {
  formatDayMonthYear,
  parseDayMonthYearToUtcDate,
} from '@common/utils/date/dayMonthYear';

describe('dayMonthYear', () => {
  describe('parseDayMonthYearToUtcDate', () => {
    test('returns fully parsed UTC date', () => {
      expect(
        parseDayMonthYearToUtcDate({
          year: 2020,
          month: 7,
          day: 13,
        }),
      ).toEqual(new Date('2020-07-13'));
    });

    test('returns parsed UTC date when there is only a year', () => {
      expect(
        parseDayMonthYearToUtcDate({
          year: 2020,
          month: null,
          day: null,
        }),
      ).toEqual(new Date('2020-01-01'));
    });

    test('returns parsed UTC date when there is only a year and month', () => {
      expect(
        parseDayMonthYearToUtcDate({
          year: 2020,
          month: 7,
          day: null,
        }),
      ).toEqual(new Date('2020-07-01'));
    });

    test('throw error if missing year', () => {
      expect(() =>
        parseDayMonthYearToUtcDate({
          year: null,
          month: 7,
          day: 13,
        }),
      ).toThrowError(/Could not parse invalid DayMonthYear to date/);
    });
  });

  describe('formatDayMonthYear', () => {
    test('returns fully formatted date using default format', () => {
      expect(
        formatDayMonthYear({
          year: 2020,
          month: 7,
          day: 13,
        }),
      ).toBe('13 July 2020');
    });

    test('returns full formatted date using custom format', () => {
      expect(
        formatDayMonthYear(
          {
            year: 2020,
            month: 7,
            day: 13,
          },
          {
            fullFormat: 'yyyy-MM-dd',
          },
        ),
      ).toBe('2020-07-13');
    });

    test('returns formatted date from only a year and month using default format', () => {
      expect(
        formatDayMonthYear({
          year: 2020,
          month: 7,
          day: null,
        }),
      ).toBe('July 2020');
    });

    test('returns formatted date from only a year and a month using custom format', () => {
      expect(
        formatDayMonthYear(
          {
            year: 2020,
            month: 7,
            day: null,
          },
          { monthYearFormat: 'yyyy-MM' },
        ),
      ).toBe('2020-07');
    });

    test('returns formatted date from only a year', () => {
      expect(
        formatDayMonthYear({
          year: 2020,
          month: null,
          day: null,
        }),
      ).toBe('2020');
    });
  });
});
