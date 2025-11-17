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
        public float error = 0;

        private float _prev_error, _integral, _memory;

        [HideInInspector]
        public float max_output;
        [HideInInspector]
        public float min_output;

        public void Reset_Memory()
        {
            //setpoint = 0;
            h = 0;
            u = 0;
            dh = 0;
            p = 0;
            error = 0;
            _prev_error = 0;
            _integral = 0;
            _memory = 0;
        }

        private float Clamp(float value)
        {
            value = value > max_output ? max_output : value;
            return value < min_output ? min_output : value;
        }

        public float Calculate()
        {
            error = setpoint - h;

            _integral += error * Time.fixedDeltaTime;

            float kp = kp_gain * error;
            float ki = ki_gain * _integral;
            float kd = kd_gain * ((error - _prev_error) / Time.fixedDeltaTime);

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
