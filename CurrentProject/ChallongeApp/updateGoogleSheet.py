import os.path
import pickle
from googleapiclient.discovery import build
from google_auth_oauthlib.flow import InstalledAppFlow
from google.auth.transport.requests import Request

def update(challongeStats, googleURL):
    creds = __authorize()
    service = build('sheets', 'v4', credentials=creds)
    sheet = service.spreadsheets()
    results = sheet.values().get(spreadsheetId=googleURL, range='A1:B2').execute()
    print(results)


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
