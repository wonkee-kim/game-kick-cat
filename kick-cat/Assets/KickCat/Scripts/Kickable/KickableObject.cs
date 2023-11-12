using UnityEngine;

public class KickableObject : MonoBehaviour
{
    private const float MAX_LIFE_TIME = 10f;
    private const float ROTATION_SPEED = 360f;

    public KickableObjectState state { get; private set; }

    [SerializeField] private KickableObjectView _view;

    private bool _isKicked = false;
    private Vector3 _velocity = Vector3.zero;
    private float _rotationVelocity = 0f;
    private float _spawnTime = 0f;

    public void Initalize(KickableObjectState state)
    {
        this.state = state;
        _isKicked = false;
        _velocity = Vector3.zero;
        _rotationVelocity = 0f;
        _spawnTime = Time.time;
        _view.Initialize(state.texture);
        _view.SetKick(false);
    }

    public void Kick(Vector3 velocity, float rotationVelocity)
    {
        _isKicked = true;
        _velocity = velocity;
        _rotationVelocity = rotationVelocity;
        _view.SetKick(true);
    }

    private void Update()
    {
        if (_isKicked)
        {
            _velocity += Vector3.down * GameSettings.gravity * Time.deltaTime;
            transform.position += _velocity * Time.deltaTime;
            transform.Rotate(Vector3.forward, ROTATION_SPEED * _rotationVelocity * Time.deltaTime);
            if (transform.position.y - state.halfSize < GameSettings.groundLevel)
            {
                transform.position = new Vector3(transform.position.x, GameSettings.groundLevel + state.halfSize, transform.position.z);
                _velocity.y *= -1;
            }
            _velocity -= _velocity * GameSettings.drag * Time.deltaTime;
            _rotationVelocity -= _rotationVelocity * GameSettings.drag * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * state.speed * GameSettings.scrollSpeed * Time.deltaTime;
        }

        if (!CheckInView())
        {
            KickablesManager.RemoveKickableObject(this);
        }
    }

    private bool CheckInView()
    {
        float timeSinceSpawn = Time.time - _spawnTime;
        // Ensure that the object is not destroyed too early
        if (timeSinceSpawn < 2f)
        {
            return true;
        }
        else if (timeSinceSpawn > MAX_LIFE_TIME)
        {
            return false;
        }
        return transform.position.x - state.halfSize < GameSettings.viewPortRange && transform.position.x + state.halfSize > -GameSettings.viewPortRange;
    }

}
