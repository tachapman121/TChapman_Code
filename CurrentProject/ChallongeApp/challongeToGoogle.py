import challonge
import configparser
import challongeStats
import updateGoogleSheet

def main():
    # Read from ./config.ini for parameters
    config = configparser.ConfigParser()
    config.read("./config.ini")
    username=config.get('SectionOne', 'username')
    api = config.get('SectionOne', 'api_key')
    challongeURL = config.get('SectionOne', 'challongeURL')
    googleURL = config.get('SectionOne', 'googleURL')

    __checkVariables(username, api, challongeURL, googleURL)
    results = challongeStats.getChallongeStats(username, api, challongeURL)
    print("Successfully got Challonge Results")
    updateGoogleSheet.update(results, googleURL)
    print("Successfully updated Google Doc at " + googleURL)

def __checkVariables(username, api_key, challongeURL, googleURL):
    message = 'Please enter parameter in config.ini under [SectionOne] and run again'
    if username == None:
        print('Username is empty. ' + message)
        exit(0)
    elif api_key == None:
        print('api_key is empty. ' + message)
        exit(0)
    elif challongeURL == None:
        print('challongeURL is empty. ' + message)
        exit(0)
    elif googleURL == None:
        print('googleURL is empty. ' + message)
        exit(0)

    print("Variables working, getting challonge results for tournament " + challongeURL)

if __name__ == "__main__":
    main()
