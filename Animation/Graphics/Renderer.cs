using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint turtleBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=-1;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 turtleCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0, 0, 0.4f, 1);
            
            float[] turtleVertices= { 
		        
				//body
				0.0f,  0.7f, 0.0f,
                0,1,0,
                0.3f,  0.4f, 0.0f,
                0,1,0,
                0.4f,  0.0f, 0.0f,
                0,1,0,
                0.3f,  -0.4f, 0.0f,
                0,1,0,
                0.0f,  -0.7f, 0.0f,
                0,1,0,
                -0.3f,  -0.4f, 0.0f,
                0,1,0,
                -0.4f,  0.0f, 0.0f,
                0,1,0,
                -0.3f,  0.4f, 0.0f,
                0,1,0,

                //head
                0.0f,  0.95f, 0.0f,
                0,1,0,
                0.12f,  0.8f, 0.0f,
                0,1,0,
                0.1f,  0.6f, 0.0f,
                0,1,0,
                0.0f,  0.7f, 0.0f,
                0,1,0,
                -0.1f,  0.6f, 0.0f,
                0,1,0,
                -0.12f,  0.8f, 0.0f,
                0,1,0,


               // right arm
               0.3f,  0.4f, 0.0f,
               0,1,0,
               0.2f,  0.5f, 0.0f,
               0,1,0,
               0.3f,  0.65f, 0.0f,
               0,1,0,
               0.45f,  0.65f, 0.0f,
               0,1,0,

               0.45f,  0.65f, 0.0f,
               0,1,0,
               0.6f,  0.5f, 0.0f,
               0,1,0,
               0.65f,  0.25f, 0.0f,
               0,1,0,
               0.5f,  0.4f, 0.0f,
               0,1,0,

                0.3f,  0.4f, 0.0f,
                0,1,0,
                0.45f,  0.65f, 0.0f,
                0,1,0,
                0.5f,  0.4f, 0.0f,
                0,1,0,
                
                 // LEFT arm
               -0.3f,  0.4f, 0.0f,
                0,1,0,
               -0.2f,  0.5f, 0.0f,
                0,1,0,
               -0.3f,  0.65f, 0.0f,
                0,1,0,
               -0.45f,  0.65f, 0.0f,
                0,1,0,


               -0.45f,  0.65f, 0.0f,
                0,1,0,
               -0.6f,  0.5f, 0.0f,
                0,1,0,
               -0.65f,  0.25f, 0.0f,
                0,1,0,
               -0.5f,  0.4f, 0.0f,
                0,1,0,


                -0.3f,  0.4f, 0.0f,
                 0,1,0,
                -0.45f,  0.65f, 0.0f,
                 0,1,0,
                -0.5f,  0.4f, 0.0f,
                 0,1,0,

                 //right leg
                 0.3f,  -0.4f, 0.0f,
                 0,1,0,
                 0.4f,  -0.65f, 0.0f,
                 0,1,0,
                 0.35f,  -0.75f, 0.0f,
                 0,1,0,
                 0.27f,  -0.75f, 0.0f,
                 0,1,0,
                 0.2f,  -0.5f, 0.0f,
                 0,1,0,

                  //left leg
                 -0.3f,  -0.4f, 0.0f,
                 0,1,0,
                 -0.4f,  -0.65f, 0.0f,
                 0,1,0,
                 -0.35f,  -0.75f, 0.0f,
                 0,1,0,
                 -0.27f,  -0.75f, 0.0f,
                 0,1,0,
                 -0.2f,  -0.5f, 0.0f,
                 0,1,0,

                 //back
                 //1
                 0.0f,  0.7f, 0.0f,
                 1,1,0,
                 0.2f,  0.5f, 0.0f,
                 1,1,0,
                 0.15f,  0.3f, 0.0f,
                 1,1,0,
                -0.15f,  0.3f, 0.0f,
                1,1,0,
                -0.2f,  0.5f, 0.0f,
                1,1,0,


                //2
                 0.2f,  0.5f, 0.0f,
                 1,1,0,
                 0.15f,  0.3f, 0.0f,
                 1,1,0,
                 0.2f,  0.2f, 0.0f,
                 1,1,0,
                 0.3f,  0.4f, 0.0f,
                 1,1,0,
                 //3
                  0.3f,  0.4f, 0.0f,
                  1,1,0,
                  0.2f,  0.2f, 0.0f,
                  1,1,0,
                  0.17f,  0.1f, 0.0f,
                  1,1,0,
                   0.25f,  0.0f, 0.0f,
                   1,1,0,
                  0.4f,  0.0f, 0.0f,
                  1,1,0,

                  //4
                   0.4f,  0.0f, 0.0f,
                   1,1,0,
                   0.25f,  0.0f, 0.0f,
                   1,1,0,
                   0.2f,  -0.2f, 0.0f,
                   1,1,0,
                   0.3f,  -0.4f, 0.0f,
                   1,1,0,
                   //5
                   0.2f,  -0.2f, 0.0f,
                   1,1,0,
                   0.15f,  -0.4f, 0.0f,
                   1,1,0,
                   0.2f,  -0.5f, 0.0f,
                   1,1,0,
                   0.3f,  -0.4f, 0.0f,
                   1,1,0,
                   //6
                   0.15f,  -0.4f, 0.0f,
                   1,1,0,
                   -0.15f,  -0.4f, 0.0f,
                   1,1,0,
                   -0.2f,  -0.5f, 0.0f,
                   1,1,0,
                   0.0f,  -0.7f, 0.0f,
                   1,1,0,
                   0.2f,  -0.5f, 0.0f,
                   1,1,0,

                   ///////////left back
                   
                //-2
                 -0.2f,  0.5f, 0.0f,
                 1,1,0,
                 -0.15f,  0.3f, 0.0f,
                 1,1,0,
                 -0.2f,  0.2f, 0.0f,
                 1,1,0,
                 -0.3f,  0.4f, 0.0f,
                 1,1,0,
                 //-3
                  -0.3f,  0.4f, 0.0f,
                  1,1,0,
                  -0.2f,  0.2f, 0.0f,
                  1,1,0,
                  -0.17f,  0.1f, 0.0f,
                  1,1,0,
                   -0.25f,  0.0f, 0.0f,
                   1,1,0,
                  -0.4f,  0.0f, 0.0f,
                  1,1,0,

                  //-4
                   -0.4f,  0.0f, 0.0f,
                   1,1,0,
                   -0.25f,  0.0f, 0.0f,
                   1,1,0,
                   -0.2f,  -0.2f, 0.0f,
                   1,1,0,
                   -0.3f,  -0.4f, 0.0f,
                   1,1,0,
                   //-5
                   -0.2f,  -0.2f, 0.0f,
                   1,1,0,
                   -0.15f,  -0.4f, 0.0f,
                   1,1,0,
                   -0.2f,  -0.5f, 0.0f,
                   1,1,0,
                   -0.3f,  -0.4f, 0.0f,
                   1,1,0,




                 //center back
                 //1
                 0.15f,  0.3f, 0.0f,
                 1,1,0,
                -0.15f,  0.3f, 0.0f,
                1,1,0,
                -0.2f,  0.2f, 0.0f,
                1,1,0,
                -0.17f,  0.1f, 0.0f,
                1,1,0,
                0.17f,  0.1f, 0.0f,
                1,1,0,
                0.2f,  0.2f, 0.0f,
                1,1,0,


                //2
                0.17f,  0.1f, 0.0f,
                1,1,0,
               -0.17f,  0.1f, 0.0f,
                1,1,0,
               -0.25f,  0.0f, 0.0f,
                1,1,0,
               -0.2f,  -0.2f, 0.0f,
                1,1,0,
                0.2f,  -0.2f, 0.0f,
                1,1,0,
                0.25f,  0.0f, 0.0f,
                1,1,0,
                //3
                0.2f,  -0.2f, 0.0f,
                1,1,0,
               -0.2f,  -0.2f, 0.0f,
                1,1,0,
               -0.15f,  -0.4f, 0.0f,
                1,1,0,
                0.15f,  -0.4f, 0.0f,
                1,1,0,


            }; // turtle Center = (10, 7, -5)
            
            turtleCenter = new vec3(10, 7, -5);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        200.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f, 
		        0.0f, 200.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, -200.0f,
                0.0f, 0.0f, 1.0f,  
            };


            turtleBufferID = GPU.GenerateBuffer(turtleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(1, 1, 1), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated turtle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, turtleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));


            // body
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 8);
            // head
            Gl.glDrawArrays(Gl.GL_POLYGON, 8, 6);
            // arm
            Gl.glDrawArrays(Gl.GL_POLYGON, 14, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 18, 4);      //right arm
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 22, 3);

            Gl.glDrawArrays(Gl.GL_POLYGON, 25, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 29, 4);      //left arm
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 33, 3);

            // leg
            Gl.glDrawArrays(Gl.GL_POLYGON, 36, 5);//riht
            Gl.glDrawArrays(Gl.GL_POLYGON, 41, 5);//left

            //back
            Gl.glDrawArrays(Gl.GL_POLYGON, 46, 5);    //1
            Gl.glDrawArrays(Gl.GL_POLYGON, 51, 4);    //2
            Gl.glDrawArrays(Gl.GL_POLYGON, 55, 5);    //3
            Gl.glDrawArrays(Gl.GL_POLYGON, 60, 4);    //4
            Gl.glDrawArrays(Gl.GL_POLYGON, 64, 4);    //5
            Gl.glDrawArrays(Gl.GL_POLYGON, 68, 5);    //6
            Gl.glDrawArrays(Gl.GL_POLYGON, 73, 4);    //-2
            Gl.glDrawArrays(Gl.GL_POLYGON, 77, 5);    //-3
            Gl.glDrawArrays(Gl.GL_POLYGON, 82, 4);    //-4
            Gl.glDrawArrays(Gl.GL_POLYGON, 86, 4);    //-5

            //centre back
            Gl.glDrawArrays(Gl.GL_POLYGON, 90, 6);
            Gl.glDrawArrays(Gl.GL_POLYGON, 96, 6);
            Gl.glDrawArrays(Gl.GL_POLYGON, 102, 4);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/2000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
           // transformations.Add(glm.translate(new mat4(1), -1 * turtleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, -1)));
          //transformations.Add(glm.translate(new mat4(1),  turtleCenter));
           transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
