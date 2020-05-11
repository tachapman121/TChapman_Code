from datetime import datetime, timedelta 
import os.path
import pickle
from googleapiclient.discovery import build
from google_auth_oauthlib.flow import InstalledAppFlow
from google.auth.transport.requests import Request

def update(challongeStats, googleURL):
    creds = __authorize()
    service = build('sheets', 'v4', credentials=creds)
    sheet = service.spreadsheets()
            
    # add new worksheet (tab) with currentDate-1 as title in mm-dd format
    dt_title = (datetime.today() - timedelta(1)).strftime("%m-%d")
    requests = [{'addSheet': {'properties': {'title': dt_title}}}]
    body = {'requests': requests}
    res = sheet.batchUpdate(spreadsheetId=googleURL, body=body).execute()
    sheetId = res['replies'][0]
    sheetId = sheetId['addSheet']['properties']['sheetId']

    # print(res)

    # loop through and append to values
    values = []
    values.append(["Name", "Placement", "Points", "Set Wins", "Set Losses", "Win%", "GameWins", "GameLosses", "Lost To"])
    for key in challongeStats:
        stats = []
        stats.append(challongeStats[key]['name'])
        stats.append(challongeStats[key]['rank'])
        stats.append(challongeStats[key]['points'])
        stats.append(challongeStats[key]['wins'])
        stats.append(challongeStats[key]['losses'])
        stats.append(challongeStats[key]['winPercentage'])
        stats.append(challongeStats[key]['gameWins'])
        stats.append(challongeStats[key]['gameLosses'])
        lostTo = ''
        for loss in challongeStats[key]['lostTo']:
            lostTo += loss + ','

        stats.append(lostTo)
        values.append(stats)

    # print(stats)

    # setup request and update sheet
    ranges = dt_title + "!A1:ZZ5"
    body = {'majorDimension': 'ROWS', 'values': values}
    results = sheet.values().append(spreadsheetId=googleURL, valueInputOption='RAW', insertDataOption='OVERWRITE',  range=ranges, body=body).execute()
    # print(results)

    # add any additional styling if needed
    __style(sheet, googleURL, sheetId)


# Example taken from Google Sheet API docs
# NOTE: REQUIRES ENABLING GOOGLES SHEETS API. TO DO SO
#       GO HERE: https://developers.google.com/sheets/api/quickstart/python
#       
def __authorize():
    creds = None
    if os.path.exists('token.pickle'):
        with open('token.pickle', 'rb') as token:
            creds = pickle.load(token)
    # If there are no (valid) credentials available, let the user log in.
    if not creds or not creds.valid:
        if creds and creds.expired and creds.refresh_token:
            creds.refresh(Request())
        else:
            # authorizes access to Google Sheets only
            flow = InstalledAppFlow.from_client_secrets_file(
                'credentials.json', 'https://www.googleapis.com/auth/spreadsheets')
            creds = flow.run_local_server(port=0)
        # Save the credentials for the next run
        with open('token.pickle', 'wb') as token:
            pickle.dump(creds, token)

    return creds

def __style(sheet, googleURL, sheetId):
    requests = []
    
    # sort spreadsheet by point values
    request = {
        "sortRange": {
            "range": {
                "sheetId": sheetId, 
                "startRowIndex": 1, #ignore header
                "endRowIndex": 200,
                "startColumnIndex": 0, 
                "endColumnIndex": 10
                }, 
            "sortSpecs": [{
                    "dimensionIndex": 1, # assumes points is second column
                    "sortOrder": "ASCENDING"
                    }, {
                    "dimensionIndex": 0, # name, 
                    "sortOrder": "ASCENDING"
                    }]
            }
        }

    requests.append(request)

    #repeat as needed...

    body = {"requests": requests}
    sheet.batchUpdate(spreadsheetId=googleURL, body=body).execute()
    return

# for testing, doesn't supply results if run as __main__
if(__name__ == '__main__'):
    main()
def main():
    import configparser
    config = configparser.ConfigParser()
    config.read("./config.ini")
    googleURL = config.get('SectionOne', 'googleURL')    
    update(None, googleURL)
