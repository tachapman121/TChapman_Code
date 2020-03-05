import challonge

def getChallongeStats(username, api, challongeURL):
    challonge.set_credentials(username, api)
    try:
        tournament = challonge.tournaments.show(challongeURL)
    except:
        print("URL invalid. challongeURL should be value after / . If a sub-domain is used (https://subdomain.challonge.com/123) it should be listed as subdomain-123")
        print("Please fix the challongeURL in config.ini and run again")
        exit(2)

    participantList = challonge.participants.index(tournament["id"])
    matches = challonge.matches.index(tournament["id"])
    
    numberOfParticipants = len(participantList)
    pointList = __setPoints(numberOfParticipants)
    stats = {}

    # update particiapnt stats: rank, name, points gained. Set properties for win/loss/% for later
    for participant in participantList:
        rank = participant["final-rank"]
        name = participant["username"]
        id = participant["id"]
        points = __givePoints(rank, pointList)
        stats[id] = ({'rank': rank, 'name': name, 'points': points, 'wins': 0, 'losses': 0, 'gameWins': 0, 'gameLosses': 0, 'winPercentage': 0, 'lostTo': []})

    for match in matches:
        p1 = match["player1-id"]
        p2 = match["player2-id"]

        # comes in as "2-1" need to split on -
        score = match["scores-csv"].split('-')

        p1Stats = stats[p1]
        p2Stats = stats[p2]

        # if score is 2-1, p1 won 2 and lost 1; update stats
        p1Stats["gameWins"] += int(score[0])
        p2Stats["gameWins"] += int(score[1])
        p1Stats["gameLosses"] += int(score[1])
        p2Stats["gameLosses"] += int(score[0])

        # update set win/losses and who lost to
        if score[0] > score[1]:
            p1Stats['wins'] += 1
            p2Stats['losses'] += 1
            p2Stats['lostTo'].append(p1Stats['name'])
        else:
            p1Stats['losses'] += 1
            p2Stats['wins'] += 1
            p1Stats['lostTo'].append(p2Stats['name'])
        
        p1Stats["winPercentage"] = round((p1Stats["wins"] / (p1Stats["wins"] + p1Stats["losses"])), 2)
        p2Stats["winPercentage"] = round((p2Stats["wins"] / (p2Stats["wins"] + p2Stats["losses"])), 2)
    
    # print(stats)
    return stats

# param: Number of particiapnts (int)
def __setPoints(participants):
    points = []
    if participants <= 8:
        points = [50, 40, 30, 20, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0]
    elif participants < 12:
        points = [60, 50, 40, 30, 20, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0]
    elif participants < 16:
        points = [80, 60, 50, 40, 30, 20, 10, 5, 0, 0, 0, 0, 0, 0, 0]
    elif participants < 24:
        points = [100, 80, 60, 50, 40, 30, 20, 10, 5, 0, 0, 0, 0, 0, 0]
    elif participants < 32:
        points = [130, 100, 80, 60, 50, 40, 30, 20, 10, 5, 0, 0, 0, 0, 0]
    elif participants < 48:
        points = [160, 130, 100, 80, 60, 50, 40, 30, 20, 10, 5, 0, 0, 0, 0]
    elif participants < 64:
        points = [200, 160, 130, 100, 80, 60, 50, 40, 30, 20, 10, 5, 0, 0, 0]
    elif participants < 96:
        points = [300, 200, 160, 130, 100, 80, 60, 50, 40, 30, 20, 10, 5, 0, 0]
    elif participants < 128:
        points = [500, 300, 200, 160, 130, 100, 80, 60, 50, 40, 30, 20, 10, 5, 0]
    elif participants < 196:
        points = [1000, 500, 300, 200, 160, 130, 100, 80, 60, 50, 40, 30, 20, 10, 5]

    return points;

def __givePoints(rank, points):
    if rank == 1:
        return points[0]
    elif rank == 2:
        return points[1]
    elif rank == 3:
        return points[2]
    elif rank == 4:
        return points[3]
    elif rank == 5:
        return points[4]
    elif rank == 7:
        return points[5]
    elif rank == 9:
        return points[6]
    elif rank == 13:
        return points[7]
    elif rank == 17:
        return points[8]
    elif rank == 25:
        return points[9]
    elif rank == 33:
        return points[10]
    elif rank == 49:
        return points [11]
    elif rank == 65:
        return points[12]
    elif rank == 97:
        return points[13]
    elif rank == 129:
        return points[14]
    else:
        return 0

if (__name__ == '__main__'):
    main()

def main():
    import configparser
    config = configparser.ConfigParser()
    config.read("./config.ini")
    username=config.get('SectionOne', 'username')
    api = config.get('SectionOne', 'api_key')
    challongeURL = config.get('SectionOne', 'challongeURL')
    return getChallongeStats(username, api, challongeURL)
    
