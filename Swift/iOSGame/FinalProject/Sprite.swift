//
//  Sprite.swift
//  FinalProject
//
//  Created by Trevor Chapman on 4/23/17.
//  Copyright © 2017 Trevor Chapman. All rights reserved.
//

import GLKit

class Sprite
{
    var positionX: Float = 1.5
    var positionY: Float = 1.5
    var scaleX: Float = 1.0
    var scaleY: Float = 1.0
    var rotation: Float = 0.0
    
    private let texture1: GLKTextureInfo?
    private let texture2: GLKTextureInfo?
    private var textureNumber: Int = 0
    
    private static var program: GLuint = 0
    
    private var _quad: [Float] = []
    
    init(image: UIImage, image2: UIImage?, quad: [Float])
    {
        _quad = quad
        texture1 = try? GLKTextureLoader.texture(with: image.cgImage!, options: nil)
        if (image2 != nil) {
            texture2 = try? GLKTextureLoader.texture(with: image2!.cgImage!, options: nil) }
        else {
            texture2 = nil }
        
        Sprite.setup()
    }
    
    func draw()
    {
        glUseProgram(Sprite.program)
        
        glVertexAttribPointer(0, 2, GLenum(GL_FLOAT), GLboolean(GL_FALSE), 32, _quad)
        glVertexAttribPointer(1, 4, GLenum(GL_FLOAT), GLboolean(GL_FALSE), 32, UnsafePointer((_quad)) + 2)
        glVertexAttribPointer(2, 2, GLenum(GL_FLOAT), GLboolean(GL_FALSE), 32, UnsafePointer(_quad) + 6)
        
        glUniform2f(glGetUniformLocation(Sprite.program, "translate"), positionX, positionY)
        glUniform2f(glGetUniformLocation(Sprite.program, "scale"), scaleX, scaleY)
        glUniform1f(glGetUniformLocation(Sprite.program, "rotation"), rotation)
        
        if let texture = texture1 {
            glBindTexture(GLenum(GL_TEXTURE_2D), texture.name) }
        
        glDrawArrays(GLenum(GL_TRIANGLE_STRIP), 0, 4)
    }
    
    private static func setup()
    {
        // makes so only runs once
        if (program != 0) {
            return }
        
        let shaderPath: String = Bundle.main.path(forResource: "sprite_vertex", ofType: "glsl", inDirectory: nil)!
        let shaderSource: NSString = try! NSString(contentsOfFile: shaderPath, encoding: String.Encoding.utf8.rawValue)
        var vertexShaderData = shaderSource.cString(using: String.Encoding.utf8.rawValue)
        
        let shader: GLuint = glCreateShader(GLenum(GL_VERTEX_SHADER))
        
        glShaderSource(shader, 1, &vertexShaderData, nil)
        glCompileShader(shader)
        
        var shaderCompileStatus: GLint = GL_FALSE
        glGetShaderiv(shader, GLenum(GL_COMPILE_STATUS), &shaderCompileStatus)
        if shaderCompileStatus == GL_FALSE {
            var logLength: GLint = 0
            glGetShaderiv(shader, GLenum(GL_INFO_LOG_LENGTH), &logLength)
            let logBuffer = UnsafeMutablePointer<GLchar>.allocate(capacity: Int(logLength))
            
            glGetShaderInfoLog(shader, logLength, nil, logBuffer)
            let logString: String = String(cString: logBuffer)
            
            NSLog("\(logString)")
            fatalError("First shader couldn't compile")
        }
        
        let fragmentShaderPath: String = Bundle.main.path(forResource: "sprite_fragment", ofType: "glsl", inDirectory: nil)!
        let fragmentShaderSource: NSString = try! NSString(contentsOfFile: fragmentShaderPath, encoding: String.Encoding.utf8.rawValue)
        var fragmentShaderData = fragmentShaderSource.cString(using: String.Encoding.utf8.rawValue)
        
        let fragmentShader: GLuint = glCreateShader(GLenum(GL_FRAGMENT_SHADER))
        
        glShaderSource(fragmentShader, 1, &fragmentShaderData, nil)
        glCompileShader(fragmentShader)
        
        glGetShaderiv(fragmentShader, GLenum(GL_COMPILE_STATUS), &shaderCompileStatus)
        if shaderCompileStatus == GL_FALSE {
            var logLength: GLint = 0
            glGetShaderiv(shader, GLenum(GL_INFO_LOG_LENGTH), &logLength)
            let logBuffer = UnsafeMutablePointer<GLchar>.allocate(capacity: Int(logLength))
            
            glGetShaderInfoLog(shader, logLength, nil, logBuffer)
            let logString: String = String(cString: logBuffer)
            
            NSLog("\(logString)")
            fatalError("Second shader couldn't compile")
        }
        
        program = glCreateProgram()
        glAttachShader(program, shader)
        glAttachShader(program, fragmentShader)
        glBindAttribLocation(program, 0, "position")
        glBindAttribLocation(program, 1, "color")
        glBindAttribLocation(program, 2, "texturePos")
        
        glLinkProgram(program)
        
        var linkStatus: GLint = GL_FALSE
        glGetProgramiv(program, GLenum(GL_LINK_STATUS), &linkStatus)
        if linkStatus == GL_FALSE {
            var logLength: GLint = 0
            glGetProgramiv(program, GLenum(GL_INFO_LOG_LENGTH), &logLength)
            let logBuffer = UnsafeMutablePointer<GLchar>.allocate(capacity: Int(logLength))
            
            let logString: String = String(cString: logBuffer)
            
            NSLog("\(logString)")
            fatalError("Linking Error")
        }
        
        glUseProgram(program)
        
        glEnableVertexAttribArray(0)
        glEnableVertexAttribArray(1)
        glEnableVertexAttribArray(2)
    }
}


















