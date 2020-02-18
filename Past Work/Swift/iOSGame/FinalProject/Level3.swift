//
//  Level3.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/25/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import Foundation

class Level3: Level
{
    private var _time: Float
    
    private var _nextTimes: [LevelTimes]
    private var _nextTime: Int
    private var _delegate: GameDelegate?
    
    var time: Float { get { return _time } }
    var delegate: GameDelegate {
        get { return _delegate! }
        set { _delegate = newValue }
    }
    
    init()
    {
        _time = 0
        _nextTimes = [LevelTimes(timing: 5.0, type: enemyDict),
                      LevelTimes(timing: 8.0, type: enemyDict),
                      LevelTimes(timing: 12.0, type: enemyDict),
                      LevelTimes(timing: 14.0, type: enemyDict),
                      LevelTimes(timing: 15.0, type: enemyDict),
                      LevelTimes(timing: 18.0, type: asteroidDict),
                      LevelTimes(timing: 23.0, type: enemyDict),
                      LevelTimes(timing: 0.0, type: "end")]
        _nextTime = 0
    }
    
    func reset()
    {
        _time = 0
        _nextTime = 0
    }
    
    func update(time: Float)
    {
        var paths: [Path] = []
        
        if (_nextTimes[_nextTime].time == 0.0) {
            _delegate?.endLevel() }
            
        else if (_time >= _nextTimes[_nextTime].time)
        {
            paths = generatePath(number: _nextTime, type: _nextTimes[_nextTime].object)
            switch _nextTimes[_nextTime].object
            {
            case enemyDict:
                delegate.generateEnemy(path: paths)
            case asteroidDict:
                delegate.generateAsteroid(path: paths)
                break
            default:
                break
            }
            
            _nextTime += 1
        }
        
        _time += time
    }
    
    private func generatePath(number: Int, type: String) -> [Path]
    {
        var paths: [Path] = []
        paths.append(Path.initialPath())
        
        switch(number)
        {
        case 0:
            paths.append(Path(x: 0.3, y: 0.0, fire: false))
            paths.append(Path(x: -0.5, y: 0.0, fire: true))
            break
        case 1:
            paths.append(Path(x: -0.8, y: 0.8, fire: false))
            paths.append(Path(x: -0.4, y: 0.4, fire: false))
            paths.append(Path(x: 0.0 , y: 0.8, fire: false))
            paths.append(Path(x: 0.4, y: 0.4, fire: false))
            paths.append(Path(x: 0.8, y: 0.4, fire: true))
            break
        case 2:
            paths.append(Path(x: 0.8, y: 0.0, fire: true))
            break
        case 3:
            paths.append(Path(x: -0.8, y: 0.0, fire: true))
            break
        case 4:
            paths.append(Path(x: 0.0, y: 0.0, fire: false))
            paths.append(Path(x: 0.0, y: 0.8, fire: true))
            break
        case 5:
            paths.append(Path(x: paths.first!._x, y: -2.0, fire: false))
        case 6:
            paths.append(Path(x: -0.8, y: 0.4, fire: true))
            paths.append(Path(x: -0.4, y: 0.4, fire: true))
            paths.append(Path(x: 0.0, y: 0.4, fire: true))
            paths.append(Path(x: 0.4, y: 0.4, fire: true))
            paths.append(Path(x: 0.8, y: 0.4, fire: false))
            break
        default:
            break;
        }
        
        if (type == enemyDict) {
            paths.append(Path.finalPath()) }
        
        return paths
    }
}
