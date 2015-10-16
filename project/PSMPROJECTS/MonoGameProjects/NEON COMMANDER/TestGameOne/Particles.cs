using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Particles
{
    /// <summary>
    /// class used for generating random values between a min and max
    /// </summary>
    public struct Range
    {
        /// <summary>
        /// float variables for storing min and max values to be generated
        /// </summary>
        public float min, max;

        /// <summary>
        /// constructor for class
        /// </summary>
        /// <param name="_min">min float value to be generated</param>
        /// <param name="_max">max float value to be generated</param>
        public Range(float _min, float _max)
        {
            min = _min;
            max = _max;
        }

        /// <summary>
        /// return a random float value between the internal min and max values
        /// </summary>
        /// <param name="rng">and rng object to use (for optimisation)</param>
        /// <returns>float</returns>
        public float Next(Random rng)
        {
            return (float)(rng.Next((int)(min * 1000), (int)(max * 1000))) / 1000f;
        }
    }

    /// <summary>
    /// enum defining direction of particle rotation
    /// </summary>
    public enum RotateDirection
    {
        CW,     // clockwise
        CCW,    // counter-clockwise
        RAND,   // random direction (set once)
    }

    /// <summary>
    /// the main particle system manager
    /// </summary>
    public class ParticleManager
    {
        //-------------------------------------------------

        #region PUBLIC_VALUES

        /// <summary>
        /// default particle texture generated in code so you 
        /// don't have to supply your own for basic particles
        /// </summary>
        public static Texture2D defaultParticle
        {
            get { return defaultParticleTexture; }
        }
        /// <summary>
        /// the number of existing particle systems
        /// </summary>
        public static int particleSystemCount
        {
            get { return particleSystems.Count; }
        }

        #endregion

        //-------------------------------------------------

        #region PUBLIC_METHODS

        /// <summary>
        /// initialises the Particles class (must be called before use)
        /// </summary>
        /// <param name="sb">SpriteBatch for drawing particles</param>
        /// <param name="gd">GraphicsDevice for creating default particle</param>
        public static void Initialise(SpriteBatch sb, GraphicsDevice gd)
        {
            if (sb == null)
                throw new Exception("cannot pass null for parameter 'sb' when calling 'Particles.Initialise'");
            if (gd == null)
                throw new Exception("cannot pass null for parameter 'gd' when calling 'Particles.Initialise'");

            spriteBatch = sb;
            particleSystems = new List<ParticleSystem>(10);
            destroySystems = new List<ParticleSystem>(5);
            rng = new Random();


            Color[] colors = new Color[64 * 64];
            Vector2 center = new Vector2(32, 32);

            for (int x = 0; x < 64; ++x)
            {
                for (int y = 0; y < 64; ++y)
                {
                    float dist = (new Vector2(x, y) - center).Length();
                    float alpha = 1f - (dist / 32f);
                    alpha = MathHelper.Clamp(alpha, 0f, 1f);

                    colors[(x * 64) + y] = new Color(255f, 255f, 255f, alpha);
                }
            }

            defaultParticleTexture = new Texture2D(gd, 64, 64, false, SurfaceFormat.Color);
            defaultParticleTexture.SetData(colors);
            defaultParticleTexture.Name = "defaultParticle";
        }

        /// <summary>
        /// updates all particles (must be called by Game1.Update)
        /// </summary>
        /// <param name="gameTime">GameTime object (can be found in Game1.Update)</param>
        public static void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new Exception("cannot pass null for parameter 'gameTime' when calling Particles.Update");
            if (spriteBatch == null)
                throw new Exception("must initialise 'Particles' with a non-null SpriteBatch object before use");

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particleSystems.Count; ++i)
                particleSystems[i].Update(dt);
            for (int i = 0; i < destroySystems.Count; ++i)
            {
                if (!destroySystems[i].isPlaying && destroySystems[i].particleCount == 0)
                {
                    DestroyActivePS(destroySystems[i]);
                    destroySystems.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// use this instead of SpriteBatch.Begin when drawing particles
        /// </summary>
        /// <param name="additive">true for for fire/smoke etc</param>
        public static void Begin(bool additive = false)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, additive ? BlendState.Additive : BlendState.NonPremultiplied);
        }

        /// <summary>
        /// use this instead of SpriteBatch.Begin when drawing particles and when a camera is required. 
        /// </summary>
        /// <param name="camera">Camera for the game</param>
        /// <param name="additive">true for for fire/smoke etc</param>
        public static void Begin(TestGameOne.Camera2D camera, bool additive = false)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, additive ? BlendState.Additive : BlendState.NonPremultiplied, null, null, null, null, camera.Transform);
        }


        /// <summary>
        /// use this instead of SpriteBatch.End when drawing particles
        /// </summary>
        public static void End()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// draws all particles (must be called by Game1.Draw)
        /// </summary>
        public static void Draw()
        {
            if (spriteBatch == null)
                throw new Exception("must initialise 'Particles' with a non-null SpriteBatch object before use");

            for (int i = 0; i < particleSystems.Count; ++i)
            {
                if (particleSystems[i].autoDraw)
                    particleSystems[i].Draw();
            }
        }

        /// <summary>
        /// creates a new ParticleSystem using the passed in Texture2D and returns it
        /// (use the returned ParticleSystem to change the particle system settings)
        /// </summary>
        /// <param name="texture">texture that each of this ParticleSystem's particles will use</param>
        /// <returns>ParticleSystem</returns>
        public static ParticleSystem CreateParticleSystem(Texture2D texture)
        {
            if (spriteBatch == null)
                throw new Exception("must initialise 'Particles' with a non-null SpriteBatch object before use");
            if (texture == null)
                throw new Exception("cannot pass null for parameter 'texture' when calling 'ParticleSystem.CreateParticleSystem'");

            ParticleSystem ps = new ParticleSystem(texture, spriteBatch, rng);
            particleSystems.Add(ps);
            return ps;
        }

        /// <summary>
        /// destroys all particles and particle systems
        /// </summary>
        public static void Destroy()
        {
            foreach (ParticleSystem ps in particleSystems)
                ps.Clear();
            particleSystems.Clear();
            destroySystems.Clear();
        }

        /// <summary>
        /// removes the passed in ParticleSystem from the internal list, this will destroy 
        /// the particle system and its particles if no other references to it exists
        /// </summary>
        /// <param name="ps">ParticleSystem to destroy</param>
        public static void DestroyActivePS(ParticleSystem ps)
        {
            if (ps != null && particleSystems.Contains(ps))
                particleSystems.Remove(ps);
        }

        /// <summary>
        /// removes the passed in ParticleSystem from the internal list once 
        /// it is stopped and has no active particles, this will destroy the
        /// particle system and its particles if no other references to it exists
        /// </summary>
        /// <param name="ps">ParticleSystem to destroy</param>
        public static void DestroyIdlePS(ParticleSystem ps)
        {
            if (!ps.isPlaying && ps.particleCount == 0)
                DestroyActivePS(ps);
            else if (!destroySystems.Contains(ps))
                destroySystems.Add(ps);
        }

        #endregion

        //-------------------------------------------------

        #region PRIVATE_VALUES

        private static SpriteBatch spriteBatch = null;
        private static List<ParticleSystem> particleSystems;
        private static List<ParticleSystem> destroySystems;
        private static Random rng;
        private static Texture2D defaultParticleTexture = null;

        #endregion

        //-------------------------------------------------
    }

    /// <summary>
    /// class that spawns, updates, draws and removes particles
    /// (particles spawned by this object are spawned with its settings)
    /// </summary>
    public class ParticleSystem
    {
        //-------------------------------------------------

        #region PARTICLE_SETTINGS

        /// <summary>
        /// how long particles last (default: 1)
        /// </summary>
        public Range lifeTime = new Range(1, 1);

        /// <summary>
        /// starting scale of particles (default: 1)
        /// </summary>
        public Range startSize = new Range(1, 1);
        /// <summary>
        /// end scale of particles (default: 1)
        /// </summary>
        public Range endSize = new Range(1, 1);
        /// <summary>
        /// normalised value for when in a particles lifetime it will reach its end size (default: 1)
        /// </summary>
        public float normSize = 1;

        /// <summary>
        /// starting rotation of particles (default: 0)
        /// </summary>
        public Range startRotation = new Range(0, 0);
        /// <summary>
        /// end rotation of particles (default: 0)
        /// </summary>
        public Range endRotation = new Range(0, 0);
        /// <summary>
        /// normalised value for when in a particles lifetime it will reach its end rotation (default: 1)
        /// </summary>
        public float normRotation = 1;

        /// <summary>
        /// starting speed of particles (default: 0)
        /// </summary>
        public Range startSpeed = new Range(0, 0);
        /// <summary>
        /// end speed of particles (default: 0)
        /// </summary>
        public Range endSpeed = new Range(0, 0);
        /// <summary>
        /// normalised value for when in a particles lifetime it will reach its end speed (default: 1)
        /// </summary>
        public float normSpeed = 1;

        /// <summary>
        /// starting alpha of particles (default: 1)
        /// </summary>
        public Range startAlpha = new Range(1, 1);
        /// <summary>
        /// end alpha of particles (default: 0)
        /// </summary>
        public Range endAlpha = new Range(0, 0);
        /// <summary>
        /// normalised value for when in a particles lifetime it will reach its end alpha (default: 1)
        /// </summary>
        public float normAlpha = 1;

        /// <summary>
        /// starting color of particles (default: White)
        /// </summary>
        public Color startColor = Color.White;
        /// <summary>
        /// end color of particles (default: White)
        /// </summary>
        public Color endColor = Color.White;
        /// <summary>
        /// normalised value for when in a particles lifetime it will reach its end color (default: 1)
        /// </summary>
        public float normColor = 1;

        /// <summary>
        /// start value for speed based stretching (scaling on texture Y axis) of particles (default: 1)
        /// </summary>
        public Range startStretch = new Range(1, 1);
        /// <summary>
        /// end value for speed based stretching (scaling on texture Y axis) of particles (default: 1)
        /// </summary>
        public Range endStretch = new Range(1, 1);
        /// <summary>
        /// normalised value for when in a particles lifetime it will reach its end stretch (default: 1)
        /// </summary>
        public float normStretch = 1;

        /// <summary>
        /// which direction rotating particles spawned by this particle system will rotate (default: RAND)
        /// </summary>
        public RotateDirection rotateDirection = RotateDirection.RAND;

        /// <summary>
        /// Sets whether the particles will rotate randomly or not
        /// Default is false, if false then the particle will rotate to the direction it is travelling in
        /// </summary>
        public bool randomAngle = false;

        #endregion

        //-------------------------------------------------

        #region PUBLIC_VALUES

        /// <summary>
        /// should the particles controlled by this system be drawn via Particles.Draw ?
        /// (you can call ParticleSystem.Draw yourself to control its draw order)
        /// </summary>
        public bool autoDraw = true;
        /// <summary>
        /// if true, particles will move relative to their 
        /// parent particle system's position and rotation
        /// </summary>
        public bool localTranslate = false;

        /// <summary>
        /// how many seconds will particles be spawned for, when ParticleSystem.Play is called?
        /// </summary>
        public float duration = 1;
        /// <summary>
        /// how many seconds between particle spawns while playing?
        /// </summary>
        public float rate = 0.02f;
        /// <summary>
        /// normalised speed of particle system and its particles (default: 1)
        /// [0.5: half-speed] [1.0: normal speed] [2.0: double speed]
        /// </summary>
        public float normalisedSpeed = 1;
        /// <summary>
        /// does the particle system loop playing until paused/stopped ?
        /// </summary>
        public bool looping = false;
        /// <summary>
        /// how far away (as a radius) from the particle system's position a particle can be spawned
        /// </summary>
        public float spawnArea = 0;
        /// <summary>
        /// the angle (in radians) that a spawned particle's direction can differ from its particle systems's rotation
        /// </summary>
        public float spawnAngle = (float)Math.PI * 2;
        /// <summary>
        /// maximum number of particles that can be exist under this particle system
        /// </summary>
        public int maxParticles = 1000;

        /// <summary>
        /// the number of particles that currently exist in this particle system
        /// </summary>
        public int particleCount
        {
            get
            {
                return particles.Count;
            }
        }
        /// <summary>
        /// whether or not the particle system is currently playing
        /// </summary>
        public bool isPlaying
        {
            get
            {
                if (!paused && playing)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// whether or not the particle system is currently paused
        /// </summary>
        public bool isPaused
        {
            get
            {
                if (paused && playing)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// whether or not the particle system is currently stopped
        /// </summary>
        public bool isStopped
        {
            get
            {
                if (paused && !playing)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// how many seconds the particle system has been playing
        /// </summary>
        public float time
        {
            get
            {
                return durationCount;
            }
        }

        /// <summary>
        /// position of particle system, move this to change spawn location
        /// </summary>
        public Vector2 position = Vector2.Zero;
        /// <summary>
        /// facing rotation (in radians) of the particle system, change this to change the spawnAngle direction
        /// </summary>
        public float rotation = 0;

        #endregion

        //-------------------------------------------------

        #region PUBLIC_METHODS

        /// <summary>
        /// constructor for ParticleSystem 
        /// </summary>
        /// <param name="tex">texture to be used by ParticleSystem</param>
        /// <param name="sb">SpriteBatch for drawing particles</param>
        /// <param name="_rng">rng to use</param>
        public ParticleSystem(Texture2D tex, SpriteBatch sb, Random _rng)
        {
            texture = tex;
            spriteBatch = sb;
            rng = _rng;
            particles = new List<Particle>(maxParticles);
        }

        /// <summary>
        /// draws all particles
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < particles.Count; ++i)
                particles[i].Draw();
        }

        /// <summary>
        /// plays the particle system (resets duration if not paused)
        /// </summary>
        public void Play()
        {
            if (!paused)
            {
                durationCount = 0;
            }
            paused = false;
            playing = true;
        }

        /// <summary>
        /// pauses particle system without resetting duration
        /// </summary>
        public void Pause()
        {
            paused = true;
        }

        /// <summary>
        /// stops particle system and resets duration
        /// </summary>
        public void Stop()
        {
            paused = true;
            playing = false;
            durationCount = duration;
            rateCount = 0;
        }

        /// <summary>
        /// instantly spawns a given number of particles
        /// </summary>
        /// <param name="count">number of particles to be spawned</param>
        public void Emit(int count)
        {
            emission += count;
        }

        /// <summary>
        /// clears all particles from the system
        /// </summary>
        public void Clear()
        {
            particles.Clear();
        }

        #endregion

        //-------------------------------------------------

        #region PRIVATE_VALUES

        private Texture2D texture = null;
        private SpriteBatch spriteBatch = null;
        private List<Particle> particles;
        private Random rng = null;
        private bool paused = false;
        private bool playing = false;
        private float durationCount = 0;
        private int spawn = 0;
        private float rateCount = 0;
        private int emission = 0;
        private Vector2 prePosition = Vector2.Zero;
        private Vector2 deltaPosition = Vector2.Zero;
        private float preRotation = 0;
        private float deltaRotation = 0;
        private bool virgin = true;

        #endregion

        //-------------------------------------------------

        #region PRIVATE_METHODS

        internal void Update(float dt)
        {
            dt *= normalisedSpeed;

            if (!paused && playing)
            {
                UpdateDuration(dt);
                UpdateRate(dt);
            }

            UpdateSpawn();
            UpdateEmit();
            UpdateTranslateDelta();
            UpdateParticles(dt);
        }

        private void UpdateTranslateDelta()
        {
            deltaPosition = position - prePosition;
            prePosition = position;
            deltaRotation = rotation - preRotation;
            preRotation = rotation;
        }
        private void UpdateParticles(float dt)
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                particles[i].Update(dt, deltaPosition, deltaRotation, virgin ? false : localTranslate);

                if (!particles[i].alive)
                    particles.RemoveAt(i--);
            }

            virgin = false;
        }
        private void UpdateDuration(float dt)
        {
            durationCount += dt;
            if (durationCount > duration)
            {
                if (!looping)
                    playing = false;
                durationCount = 0;
            }
        }
        private void UpdateRate(float dt)
        {
            rateCount += dt;
            while (rateCount > rate && playing)
            {
                spawn++;
                rateCount -= rate;
            }
        }
        private void UpdateSpawn()
        {
            while (spawn > 0)
            {
                Spawn();
                spawn--;
            }
        }
        private void UpdateEmit()
        {
            if (emission <= 0)
                return;

            for (int i = 0; i < 15; ++i)
            {
                Spawn();
                emission -= 1;
            }            

        }
        private void Spawn()
        {
            if (particles.Count >= maxParticles)
                return;

            float x = (float)rng.Next(-1000, 1000) / 1000;
            float y = (float)rng.Next(-1000, 1000) / 1000;
            float a = (float)rng.Next(0, (int)(spawnArea * 1000)) / 1000;
            Vector2 offset = new Vector2(x, y);
            offset.Normalize();
            offset *= a;

            Particle particle = new Particle(texture, spriteBatch, position + offset, rotation);

            if (rotateDirection == RotateDirection.RAND)
            {
                int roll = rng.Next(0, 2);
                particle.rotateDirection = roll == 0 ? RotateDirection.CW : RotateDirection.CCW;
            }
            else
                particle.rotateDirection = rotateDirection;

            particle.lifeTime = lifeTime.Next(rng);

            particle.startSize = startSize.Next(rng);
            particle.endSize = endSize.Next(rng);
            particle.normSize = normSize;

            particle.startRotation = startRotation.Next(rng);
            particle.endRotation = endRotation.Next(rng);
            particle.normRotation = normRotation;

            particle.startSpeed = startSpeed.Next(rng);
            particle.endSpeed = endSpeed.Next(rng);
            particle.normSpeed = normSpeed;

            particle.startAlpha = startAlpha.Next(rng);
            particle.endAlpha = endAlpha.Next(rng);
            particle.normAlpha = normAlpha;

            particle.startColor = startColor;
            particle.endColor = endColor;
            particle.normColor = normColor;

            particle.startStretch = startStretch.Next(rng);
            particle.endStretch = endStretch.Next(rng);
            particle.normStretch = normStretch;


            int angle = (int)(spawnAngle / 2 * 1000);
            float rot = rotation + ((float)rng.Next(-angle, angle) / 1000);
            particle.direction = new Vector2((float)Math.Sin(rot), (float)-Math.Cos(rot));
            particle.facingDirection = rot;
            particle.randomDirection = randomAngle;


            particle.PreWarm();
            particles.Add(particle);
        }

        #endregion

        //-------------------------------------------------
    }

    internal class Particle
    {
        //-------------------------------------------------

        #region PRIVATE_VALUES

        internal bool alive = true;
        internal float lifeTime = 0;
        internal float startSize = 1;
        internal float endSize = 1;
        internal float normSize = 1;
        internal float startRotation = 0;
        internal float endRotation = 0;
        internal float normRotation = 1;
        internal float startSpeed = 1;
        internal float endSpeed = 1;
        internal float normSpeed = 1;
        internal float startAlpha = 1;
        internal float endAlpha = 0;
        internal float normAlpha = 1;
        internal Color startColor = Color.White;
        internal Color endColor = Color.White;
        internal float normColor = 1;
        internal float startStretch = 1;
        internal float endStretch = 1;
        internal float normStretch = 1;
        internal RotateDirection rotateDirection = RotateDirection.CW;
        internal Vector2 direction = Vector2.Zero;

        private ParticleSprite sprite = null;
        private float lifeCount = 0;
        private float speed = 1;
        private float alpha = 1;
        private float rotation = 0;

        internal float facingDirection = 0;
        internal bool randomDirection = false;

        #endregion

        //-------------------------------------------------

        #region PRIVATE_METHODS

        internal Particle(Texture2D tex, SpriteBatch sb, Vector2 pos, float rot)
        {
            sprite = new ParticleSprite(tex, sb, pos, rot, Color.White, new Vector2(tex.Width / 2, tex.Height / 2), Vector2.One);
            rotation = rot;
        }
        internal void Update(float dt, Vector2 deltaPos, float deltaRot, bool localTrans)
        {
            if (!alive)
                return;

            UpdateSize();
            UpdateRotation();
            UpdateSpeed();
            UpdateAlpha();
            UpdateColor();
            UpdateStretch();
            UpdateMovement(dt, deltaPos, deltaRot, localTrans);
            UpdateAlive(dt);
        }
        internal void Draw()
        {
            sprite.Draw();
        }
        internal void PreWarm()
        {
            UpdateSize();
            UpdateRotation();
            UpdateSpeed();
            UpdateAlpha();
            UpdateColor();
        }

        private void UpdateSize()
        {
            float ratio = lifeCount / (lifeTime * normSize);
            ratio = MathHelper.Clamp(ratio, 0f, 1f);
            float dif = endSize - startSize;
            sprite.scale = new Vector2(startSize + (dif * ratio), startSize + (dif * ratio));
        }
        private void UpdateRotation()
        {
            if (randomDirection)
            {
                float ratio = lifeCount / (lifeTime * normRotation);
                ratio = MathHelper.Clamp(ratio, 0f, 1f);
                float dif = endRotation - startRotation;
                dif = rotateDirection == RotateDirection.CW ? dif : -dif;
                sprite.rotation = rotation + (dif * ratio);
            }

            else
                sprite.rotation = facingDirection;
        }
        private void UpdateSpeed()
        {
            float ratio = lifeCount / (lifeTime * normSpeed);
            ratio = MathHelper.Clamp(ratio, 0f, 1f);
            float dif = endSpeed - startSpeed;
            speed = startSpeed + (dif * ratio);
        }
        private void UpdateAlpha()
        {
            float ratio = lifeCount / (lifeTime * normAlpha);
            ratio = MathHelper.Clamp(ratio, 0f, 1f);
            float dif = endAlpha - startAlpha;
            alpha = startAlpha + (dif * ratio);
        }
        private void UpdateColor()
        {
            float ratio = lifeCount / (lifeTime * normColor);
            ratio = MathHelper.Clamp(ratio, 0f, 1f);

            int difRed = endColor.R - startColor.R;
            int difGreen = endColor.G - startColor.G;
            int difBlue = endColor.B - startColor.B;

            Color col = startColor;
            col.R += (byte)(difRed * ratio);
            col.G += (byte)(difGreen * ratio);
            col.B += (byte)(difBlue * ratio);
            col.A = (byte)(alpha * 255);
            sprite.color = col;
        }
        private void UpdateStretch()
        {
            float ratio = 1f - ((speed - startSpeed) / (endSpeed - startSpeed));
            if (float.IsNaN(ratio))
                ratio = 0;
            ratio = MathHelper.Clamp(ratio, 0f, 1f);
            float dif = endStretch - startStretch;
            sprite.scale.Y *= startStretch + (dif * ratio);
        }
        private void UpdateMovement(float dt, Vector2 deltaPos, float deltaRot, bool localTrans)
        {
            direction.Normalize();
            sprite.position += direction * speed * dt;

            if (localTrans)
            {
                sprite.position += deltaPos;
                sprite.rotation += deltaRot;
            }
        }
        private void UpdateAlive(float dt)
        {
            lifeCount += dt;
            if (lifeCount > lifeTime)
                alive = false;
        }

        #endregion

        //-------------------------------------------------
    }

    internal class ParticleSprite
    {
        //-------------------------------------------------

        #region PRIVATE_VALUES

        internal string name
        {
            get { return texture.Name; }
            set { texture.Name = value; }
        }
        internal Vector2 origin;
        internal Vector2 scale;
        internal Color color;
        internal Vector2 position;
        internal float rotation;
        internal Vector2 size
        {
            get
            {
                sizeInternal.X = texture.Width * scale.X;
                sizeInternal.Y = texture.Height * scale.Y;
                return sizeInternal;
            }
        }
        internal Rectangle rect
        {
            get
            {
                rectInternal.X = (int)(position.X - origin.X);
                rectInternal.Y = (int)(position.Y - origin.Y);
                rectInternal.Width = (int)size.X;
                rectInternal.Height = (int)sizeInternal.Y;
                return rectInternal;
            }
        }
        internal bool isVisible = true;
        internal SpriteEffects flippedState = SpriteEffects.None;

        private Texture2D texture;
        private SpriteBatch spriteBatch;
        private Vector2 sizeInternal;
        private Rectangle rectInternal;

        #endregion

        //-------------------------------------------------

        #region PRIVATE_METHODS

        internal ParticleSprite(Texture2D _texture, SpriteBatch _spriteBatch, Vector2 _position, float _rotation, Color _color, Vector2 _origin, Vector2 _scale)
        {
            if (_texture == null)
                throw new Exception("cannot pass null for parameter '_texture' when constructing a 'StaticSprite'");
            if (_spriteBatch == null)
                throw new Exception("cannot pass null for parameter '_spriteBatch' when constructing a 'StaticSprite'");

            texture = _texture;
            spriteBatch = _spriteBatch;
            position = _position;

            rotation = _rotation;
            color = _color;

            origin = _origin;
            scale = _scale;

            isVisible = true;
            flippedState = SpriteEffects.None;

            sizeInternal = new Vector2(texture.Width * scale.X, texture.Height * scale.Y);
            rectInternal = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)size.X, (int)size.Y);
        }
        internal void Draw()
        {
            if (isVisible)
                spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, flippedState, 0);
        }

        #endregion

        //-------------------------------------------------
    }
}
