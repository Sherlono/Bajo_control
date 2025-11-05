using UnityEngine;

namespace jv
{
    [System.Serializable] public class PID
    {
        [Header("Ganancias")]
        public float kp_gain;
        public float ki_gain;
        public float kd_gain;

        [Header("Variables sistema")]
        public float setpoint;
        public float h;
        public float u;
        public float dh = 0, p = 0;

        private float _prev_error, _integral, _memory;

        public float max_output;
        public float min_output;

        /*private void Start()
        {
            _prev_error = setpoint - h;
            _memory = h;
        }*/

        private float Clamp(float value)
        {
            value = value > max_output ? max_output : value;
            return value < min_output ? min_output : value;
        }


        public float Calculate()
        {
            float error = setpoint - h;

            _integral += error * Time.fixedDeltaTime;

            float kp = kp_gain * error;
            float ki = ki_gain * _integral;
            float kd = kd_gain * (error - _prev_error) / Time.fixedDeltaTime;

            _prev_error = error;

            return Clamp(kp + ki + kd);
        }

        public float Integrator()
        {
            _memory += dh * Time.fixedDeltaTime;
            return _memory;
        }

    }
}
