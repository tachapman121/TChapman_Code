//
//  Game.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/11/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

/*
 * Note: Coord on all objects assumed to be between -1 and 1 with 0 being origin
 */

import Foundation

class Game: GameDelegate
{
    
    
    private var _highScore: Int
    
    private var _playerShip: PlayerShip
    private var _enemies: [Enemy]
    private var _bullets: [Bullet]
    private var _asteroids: [Asteroid]
    
    private var _levels: [Level] = []
    private var _level: Level?
    private var _advanceLevel: Bool
    private var _currentLevel: Int?
    private var _playerMovement: ShipMovementEnum
    
    private var itemsToDelete: [String : Set<Int>] = [enemyDict: [], bulletDict : [], asteroidDict : []]
    
    var player: PlayerShip { get { return _playerShip } }
    var enemies: [Enemy] { get { return _enemies } }
    var bullets: [Bullet] { get { return _bullets } }
    var asteroids: [Asteroid] { get { return _asteroids } }
    var highScore: Int { get { return _highScore } }
    
    var delegate: GameControllerDelegate?
    
    func getObjects() -> [String : [GameObject]]
    {
        let object: [String : [GameObject]] = [playerDict : [player],
                                               enemyDict : enemies,
                                               bulletDict : bullets,
                                               asteroidDict : asteroids]
        return object
    }
    
    // new game
    init()
    {
        _highScore = 0
        _playerShip = PlayerShip()
        _enemies = []
        _bullets = []
        _asteroids = []
        _advanceLevel = false
        _currentLevel = 1
        _playerMovement = .none
    }
    
    // load game
    convenience init(highScore: Int, playerShip: PlayerShip, enemies: [Enemy], bullets: [Bullet], level: Int)
    {
        self.init()
        
        _highScore = highScore
        _playerShip = playerShip
        _enemies = enemies
        _bullets = bullets
        
        _currentLevel = level
        _level = _levels[_currentLevel!-1]
    }
    
    // sets up levels since cannot do in init due to self call
    func setupLevels(levelNum: Int)
    {
        _levels = []
        _levels.append(Level1())
        _levels.append(Level2())
        _levels.append(Level3())
        _levels[0].delegate = self
        _levels[1].delegate = self
        _levels[2].delegate = self
        
        _currentLevel = levelNum
        _level = _levels[levelNum-1]
        _advanceLevel = false
        
        for lev in 0..._levels.count-1 {
            _levels[lev].delegate = self
        }
    }
    
    func Update(elapsedTime: Float)
    {
        // set so does not try to delete same item twice, due to going off screen + bullet hitting or something odd
        delete(dict: itemsToDelete)
        itemsToDelete = [enemyDict : [], bulletDict : [], asteroidDict : []]
        
        moveObjects(elapsedTime: elapsedTime)
        checkCollisions(elapsedTime: elapsedTime)
        
        // check level to see if need to spawn new enemy or something??
        if (_advanceLevel == false) {
            _level?.update(time: elapsedTime) }
        else if (_advanceLevel == true && _enemies.count == 0)
        {
            // if at end, start over. Increase speed maybe?
            if (_currentLevel == _levels.count) {
                _currentLevel = 1 }
            else
            {
                _currentLevel! += 1 }
            
            // set current level to next, and reset in cases of looping over
            _level = _levels[_currentLevel!-1]
            _level?.reset()
            _advanceLevel = false
        }
    }
    
    private func moveObjects(elapsedTime: Float)
    {
        /* move enemies, and fire bullets if needed */
        if (_enemies.count > 0)
        {
            for count in 0..._enemies.count-1
            {
                let enemy: Enemy = _enemies[count]
                enemy.move(time: elapsedTime)
                if (abs(enemy.xCoord) >= 1.5 || abs(enemy.yCoord) >= 1.5) {
                    itemsToDelete[enemyDict]?.insert(count)}
            }
        }
        
        /* Move Bullets */
        if (_bullets.count > 0)
        {
            for count in 0..._bullets.count-1
            {
                let bullet: Bullet = _bullets[count]
                bullet.move(time: elapsedTime)
                if (abs(bullet.yCoord) >= 1.5) {
                    itemsToDelete[bulletDict]?.insert(count) }
            }
        }
        
        /* Move Asteroids */
        if (_asteroids.count > 0)
        {
            for count in 0..._asteroids.count-1
            {
                let asteroid: Asteroid = _asteroids[count]
                asteroid.move(time: elapsedTime)
                if (abs(asteroid.yCoord) >= 1.5) {
                    itemsToDelete[asteroidDict]?.insert(count) }
            }
        }
        
        /* Move Player */
        switch _playerMovement
        {
        case .left:
            _playerShip.strafeLeft(elapsedTime: elapsedTime)
            break
        case .right:
            _playerShip.strafeRight(elapsedTime: elapsedTime)
            break
        case .accelerate:
            _playerShip.accelerate(elapsedTime: elapsedTime)
            break
        case .decelerate:
            _playerShip.decelerate(elapsedTime: elapsedTime)
            break
        case .fire:
            fire(bullet: _playerShip.fire())
            break
        case .none:
            break
        }
    }
    
    // helper function to check for collisions
    private func checkCollisions(elapsedTime: Float)
    {
        /* Check Enemy/Player Collision */
        if (enemies.count > 0)
        {
            for count in 0...enemies.count-1
            {
                let enemy: Enemy = enemies[count]
                if (collisions(obj1: _playerShip, obj2: enemy))
                {
                    if (_playerShip.takeDamage(amount: enemy.damage))
                    {
                        delegate?.GameOver(score: _highScore)
                        return
                    }
                    
                    itemsToDelete[enemyDict]?.insert(count)
                }
            }
        }
        
        /* Check Bullet Collisions */
        if (bullets.count > 0)
        {
            /* Check Bullet/Player Collision */
            for count in 0...bullets.count-1
            {
                let bullet: Bullet = bullets[count]
                if (collisions(obj1: _playerShip, obj2: bullet) && !bullet.isPlayers!)
                {
                    if (_playerShip.takeDamage(amount: bullet.damage)) {
                        delegate?.GameOver(score: _highScore) }
                    
                    itemsToDelete[bulletDict]?.insert(count)
                }
            }
            
            /* Check Bullet/Enemy Collision */
            if (enemies.count > 0)
            {
                for count in 0...bullets.count-1
                {
                    let bullet: Bullet = bullets[count]
                    for count2 in 0...enemies.count-1
                    {
                        let enemy: Enemy = enemies[count2]
                        if (collisions(obj1: bullet, obj2: enemy) && bullet.isPlayers!)
                        {
                            itemsToDelete[bulletDict]?.insert(count)
                            if (enemy.takeDamage())
                            {
                                itemsToDelete[enemyDict]?.insert(count2)
                                _highScore += enemy.scoreWorth
                            }
                        }
                    }
                }
            }
            
            /* Check Bullet/Asteroid Collision */
            if (asteroids.count > 0)
            {
                for count in 0...bullets.count-1
                {
                    let bullet: Bullet = bullets[count]
                    for count2 in 0...asteroids.count-1
                    {
                        let asteroid: Asteroid = asteroids[count2]
                        if (collisions(obj1: bullet, obj2: asteroid) && bullet.isPlayers!)
                        {
                            itemsToDelete[bulletDict]?.insert(count) } // asteroids destroy bullets
                    }
                }

            }
        }
        
        /* Check Asteroid/Player Collision */
        if (_asteroids.count > 0)
        {
            for count in 0...asteroids.count-1
            {
                let asteroid: Asteroid = asteroids[count]
                if (collisions(obj1: _playerShip, obj2: asteroid))
                {
                    if (_playerShip.takeDamage(amount: asteroid.damage)) {
                        delegate?.GameOver(score: _highScore) }
                    
                    itemsToDelete[asteroidDict]?.insert(count)
                }
            }
        }
    }
    
    internal func movePlayer(movement: ShipMovementEnum)
    {
        _playerMovement = movement
    }
    
    // player died; save high score and end
    private func GameOver()
    {
        // empty dictionaries?
        delegate!.GameOver(score: _highScore)
    }
    
    // adds bullet to fire
    internal func fire(bullet: Bullet)
    {
        if (bullet.isPlayers!)
        {
            if (_playerShip.bulletsFiring < _playerShip.maxBullets)
            {
                _playerShip.bulletsFiring += 1
                _bullets.append(bullet)
            }
        }
            
        else {
            _bullets.append(bullet) }
    }
    
    // removes items
    private func delete(dict: [String : Set<Int>])
    {
        var count: Int
        
        // does in reverse order to not need to worry about
        if (dict[enemyDict]!.count > 0)
        {
            //for index in dict[enemyDict]!.count...1 {
            count = dict[enemyDict]!.count
            while (count > 0)
            {
                _enemies.remove(at: count-1)
                count -= 1
            }
        }
        
        if (dict[bulletDict]!.count > 0)
        {
            count = dict[bulletDict]!.count
            while (count > 0)
            {
                if _bullets[count-1].isPlayers! {
                    _playerShip.bulletsFiring -= 1 }
                _bullets.remove(at: count-1)
                count -= 1
            }
        }
        
        if (dict[asteroidDict]!.count > 0)
        {
            count = dict[asteroidDict]!.count
            while (count > 0)
            {
                _asteroids.remove(at: count-1)
                count -= 1
            }
        }
    }
    
    func generateEnemy(path: [Path])
    {
        // currently only have 1 hp, can change later on if needed
        let enemy: Enemy = Enemy(hp: 1, path: path, x: path.first!._x, y: path.first!._y)
        enemy.delegate = self
        _enemies.append(enemy)
    }
    
    internal func generateAsteroid(path: [Path])
    {
        let asteroid: Asteroid = Asteroid(xCoord: path.first!._x, yCoord: path.first!._y)
        asteroid.delegate = self
        _asteroids.append(asteroid)
    }
    
    // checks for collisions between two objects
    private func collisions(obj1: GameObject, obj2: GameObject) -> Bool
    {
        // should be size of ship's box approximately
        return distance(startX: obj1.xCoord, endX: obj2.xCoord, startY: obj1.yCoord, endY: obj2.yCoord) <= 0.1
    }
    
    // advance to the next level
    func endLevel()
    {
        _advanceLevel = true
    }
}
