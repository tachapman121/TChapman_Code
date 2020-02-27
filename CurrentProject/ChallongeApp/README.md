Small Python project to export tournament results from https://challonge.com to a Google Sheet for stats tracking purposes. Currently records the following values:

* Challonge Account Name
* Rank (1st, 2nd, 3rd, etc)
* Wins
* Losses
* Win Percentage
* Point values (Can be adjusted based on number of entrants and reward in the challongeStats.__setPoints method)

Information is read from <b>config.ini</b> file, which should have the following format:

[SectionOne]
username: challongeUserName
api_key: ABCDEFGHIJKLMN1234
googleURL: QWERTYUIOP
challongeURL: abcdef

Challonge URL should be information after the / ; when using a sub-domain include it such as subdomain-abcdef . See https://api.challonge.com/v1/documents/tournaments/index for example.

Uses pyChallonge library created at https://github.com/russ-/pychallonge


For accessing Google Sheets, this uses OAuth2 and requires installing additional libraries, in addition to enabling Google Sheets API access and giving this app access. For more information, see the Google API Docs at https://developers.google.com/sheets/api/quickstart/python