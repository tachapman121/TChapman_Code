//
//  GameView.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/13/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import GLKit

class GameView: GLKViewController, GameControllerDelegate
{
    let thirtyDegree: Float = 0.523599
    let oneEightyDegrees: Float = 3.14159
    
    // GLKViewController gurantees this
    private var glkView: GLKView {
        return view as! GLKView }
    
    private var translateX: Float = 0
    private var translateY: Float = 0
    private var texture: GLKTextureInfo?
    
    private var program: GLuint = 0
    
    private var _game: Game?
    
    private var _playerSprite: Sprite?
    private var _playerBulletSprite: Sprite?
    private var _enemySprite: Sprite?
    private var _enemyBulletSprite: Sprite?
    private var _asteroidSprite: Sprite?
    
    private var _background1: Sprite?
    private var _background2: Sprite?
    
    private var _rightArrow: Sprite?
    private var _leftArrow: Sprite?
    private var _upArrow: Sprite?
    private var _downArrow: Sprite?
    
    private var _sprites: [Sprite] = []
    private var _enemies: [Sprite] = []
    private var _bullets: [Sprite] = []
    private var _playerBullets: [Sprite] = []
    private var _asteroids: [Sprite] = []
    
    var parentDelegate: MenuDelegate?
    
    override func viewDidLoad()
    {
        super.viewDidLoad()
        
        _game = Game()
        _game?.delegate = self
        
        // will need to change for loading
        _game?.setupLevels(levelNum: 1)
        
        glkView.context = EAGLContext(api: .openGLES2)
        EAGLContext.setCurrent(glkView.context)
        
        glEnable(GLenum(GL_BLEND))
        glBlendFunc(GLenum(GL_SRC_ALPHA), GLenum(GL_ONE_MINUS_SRC_ALPHA))
        
        _background1 = Sprite(image: #imageLiteral(resourceName: "background"), image2: nil, quad: Background.quad)
        _background2 = Sprite(image: #imageLiteral(resourceName: "background"), image2: nil, quad: Background.quad)
        
        _playerSprite = Sprite(image: #imageLiteral(resourceName: "playerShip"), image2: #imageLiteral(resourceName: "explosion_player"), quad: PlayerShip.quad)
        _enemySprite = Sprite(image: #imageLiteral(resourceName: "enemyShip"), image2: #imageLiteral(resourceName: "explosion_enemy"), quad: Enemy.quad)
        _enemyBulletSprite = Sprite(image: #imageLiteral(resourceName: "bullet_enemy"), image2: #imageLiteral(resourceName: "bullet_player"), quad: Bullet.quad)
        _leftArrow = Sprite(image: #imageLiteral(resourceName: "Arrow"), image2: nil, quad: Bullet.quad)
        _rightArrow = Sprite(image: #imageLiteral(resourceName: "Arrow"), image2: nil, quad: Bullet.quad)
        _upArrow = Sprite(image: #imageLiteral(resourceName: "Arrow"), image2: nil, quad: Bullet.quad)
        _downArrow = Sprite(image: #imageLiteral(resourceName: "Arrow"), image2: nil, quad: Bullet.quad)
        _asteroidSprite = Sprite(image: #imageLiteral(resourceName: "asteroidImg"), image2: #imageLiteral(resourceName: "asteroidImg"), quad: Asteroid.quad)
        
        for _ in 0...3
        {
            let playerBullet: Sprite = Sprite(image: #imageLiteral(resourceName: "bullet_player"), image2: #imageLiteral(resourceName: "bullet_enemy"), quad: Bullet.quad)
            _playerBullets.append(playerBullet)
        }
        
        _background1?.positionX = 0.0
        _background1?.positionY = 0.0
        _background2?.positionX = 0.0
        _background2?.positionY = 1.0
        
        _leftArrow?.rotation = oneEightyDegrees
        _rightArrow?.positionX = 0.9
        _rightArrow?.positionY = -0.5
        _leftArrow?.positionX = -0.9
        _leftArrow?.positionY = -0.5
        _upArrow?.rotation = (thirtyDegree.multiplied(by: 3))
        _upArrow?.positionX = 0.0
        _upArrow?.positionY = 0.9
        _downArrow?.positionX = 0.0
        _downArrow?.positionY = -0.9
        _downArrow?.rotation = -(thirtyDegree.multiplied(by: 3))
        
        addDefaultSprites()
    }
    
    override func viewWillAppear(_ animated: Bool)
    {
        self.isPaused = false
    }
    
    override func viewWillDisappear(_ animated: Bool)
    {
        glkView.deleteDrawable()
        self.isPaused = true
    }
    
    func reset()
    {
        _game = Game()
        _game?.delegate = self
        _game?.setupLevels(levelNum: 1)
        
    }
    
    // sprites that should always be on the screen
    func addDefaultSprites()
    {
        _sprites.append(_background1!)
        _sprites.append(_background2!)
        _sprites.append(_playerSprite!)
        _sprites.append(_leftArrow!)
        _sprites.append(_rightArrow!)
        _sprites.append(_upArrow!)
        _sprites.append(_downArrow!)
    }
    
    func update()
    {
        // update position with objects
        _sprites = []
        addDefaultSprites()
        
        let objects: [String : [GameObject]] = _game!.getObjects()
        let enemies: [GameObject] = objects[enemyDict]!
        let bullets: [GameObject] = objects[bulletDict]!
        let player: [GameObject] = objects[playerDict]!
        let asteroids: [GameObject] = objects[asteroidDict]!
        
        _playerSprite?.positionX = player[0].xCoord
        _playerSprite?.positionY = player[0].yCoord
        
        var count: Int = 1
        
        // draw enemies
        for enemy: GameObject in enemies
        {
            let sprite: Sprite
            if (count > _enemies.count) {
                sprite = Sprite(image: Enemy.getImage1, image2: Enemy.getImage2, quad: Enemy.quad)
                sprite.rotation = oneEightyDegrees
                _enemies.append(sprite)
            }
            else {
                sprite = _enemies[count-1] }
            
            sprite.positionX = enemy.xCoord
            sprite.positionY = enemy.yCoord
            _sprites.append(sprite)
            count += 1
        }
        
        // draw bullets
        var playerBullets: Int = 0
        var enemyBullets: Int = 0
        for bullet: GameObject in bullets
        {
            let bull: Bullet = bullet as! Bullet
            let sprite: Sprite
            
            if (bull.isPlayers!)
            {
                sprite = _playerBullets[playerBullets]
                playerBullets += 1
            }
            else
            {
                if (count > _bullets.count)
                {
                    sprite = Sprite(image: #imageLiteral(resourceName: "bullet_enemy"), image2: #imageLiteral(resourceName: "bullet_player"), quad: Bullet.quad)
                    sprite.rotation = oneEightyDegrees
                    _bullets.append(sprite)
                }
                else {
                    sprite = _bullets[enemyBullets] }
                
                enemyBullets += 1
            }
            
            sprite.positionX = bullet.xCoord
            sprite.positionY = bullet.yCoord
            _sprites.append(sprite)
        }
        
        for asteroid: GameObject in asteroids
        {
            let sprite: Sprite
            if (count > _asteroids.count) {
                sprite = Sprite(image: Asteroid.getImage1, image2: Asteroid.getImage2, quad: Asteroid.quad)
                _asteroids.append(sprite)
            }
            else {
                sprite = _asteroids[count-1] }
            
            sprite.positionX = asteroid.xCoord
            sprite.positionY = asteroid.yCoord
            sprite.rotation += 0.1
            _sprites.append(sprite)
            count += 1
        }
        
        _game?.Update(elapsedTime: Float(self.timeSinceLastUpdate))
    }

    override func glkView(_ view: GLKView, drawIn rect: CGRect)
    {
        glClearColor(0.75, 0.75, 0.75, 1.0)
        glClear(GLbitfield(GL_COLOR_BUFFER_BIT))
        
        
        for sprite: Sprite in _sprites {
          sprite.draw() }
        
        // scroll backgrounds each draw and move as needed
        _background1?.positionY -= 0.01
        _background2?.positionY -= 0.01
        if ((_background1?.positionY)! <= Float(-0.99)) {
            _background1?.positionY = 1.0 }
        
        if ((_background2?.positionY)! <= Float(-0.99)) {
            _background2?.positionY = 1.0 }
        
        return
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?)
    {
        // take point, convert to -1 to 1
        var x: CGFloat = view.bounds.width.divided(by: 2)
        var y: CGFloat = view.bounds.height.divided(by: 2)
        let firstPoint = touches.first?.preciseLocation(in: view)
        x = (firstPoint!.x - x) / x
        y = (firstPoint!.y - y) / y
        
        // swift coord is +y is down, GL is +y is up, so convert
        y *= -1.0
        
        
        // look if left or right of screen and move as approprite, slightly lower then where placed 
        // since positionX/Y of sprite is middle
        if (x <= -0.8)
        {
            _game!.movePlayer(movement: .left)
            _playerSprite?.rotation = thirtyDegree
        }
        else if (x >= 0.8)
        {
            _game!.movePlayer(movement: .right)
            _playerSprite?.rotation = -thirtyDegree
        }
        else if (y >= 0.8)
        {
            _game!.movePlayer(movement: .accelerate) }
            
        else if (y <= -0.8)
        {
            _game!.movePlayer(movement: .decelerate) }
        
        else {
            _game!.fire(bullet: Bullet(playerBullet: true, xCoord: _game!.player.xCoord, yCoord: _game!.player.yCoord)) }
    }
    
    override func touchesEnded(_ touches: Set<UITouch>, with event: UIEvent?)
    {
        _playerSprite?.rotation = 0.0
        _game?.movePlayer(movement: .none)
    }
    
    internal func fire(bullet: Bullet)
    {
        _game!.fire(bullet: bullet)
        return
    }
    
    internal func GameOver(score: Int)
    {
        parentDelegate?.gameOver(score: _game!.highScore)
        dismiss(animated: false, completion: nil)
        return
    }
    
    internal func movePlayer(playerMovement: ShipMovementEnum)
    {
        _game!.movePlayer(movement: playerMovement)
    }
    
    internal func getObjects() -> [String : [GameObject]]
    {
        return _game!.getObjects()
    }
    

}
