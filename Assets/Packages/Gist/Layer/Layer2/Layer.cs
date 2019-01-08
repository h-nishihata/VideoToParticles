using nobnak.Gist.Extensions.ComponentExt;
using nobnak.Gist.Primitive;
using System.Collections.Generic;
using UnityEngine;

namespace nobnak.Gist.Layer2 {

    [ExecuteInEditMode]
    public class Layer : MonoBehaviour, ILayer {

        public const float EPSILON = 1e-3f;
        public const float CIRCLE_INV_DEG = 1f / 360;

		[SerializeField]
		protected Events events;

		protected Validator validator = new Validator();

        public Layer() {
            LayerToWorld = new DefferedMatrix();
            LocalToLayer = new DefferedMatrix();
            LocalToWorld = new DefferedMatrix();
        }

        #region Unity
        protected virtual void OnEnable() {
			validator.Reset();
			validator.Validation += () => {
                transform.hasChanged = false;
                GenerateLayerData();
                NotifyLayerOnChange();
            };
			validator.SetCheckers(() => !transform.hasChanged);
			validator.Validate();
            NotifyLayerOnChange();
        }
        protected virtual void OnValidate() {
			validator.Invalidate();
        }
		protected virtual void Update() {
			validator.Validate();
		}
        protected virtual void OnDisable() {

        }
        #endregion

        #region ILayer
        public virtual Validator LayerValidator {
			get {
				return validator;
			}
		}

        public DefferedMatrix LayerToWorld { get; protected set; }
        public DefferedMatrix LocalToLayer { get; protected set; }
        public DefferedMatrix LocalToWorld { get; protected set; }

        public virtual bool Raycast(Ray ray, out float distance) {
            distance = default(float);

            var n = transform.forward;
            var c = transform.position;
            var det = Vector3.Dot(n, ray.direction);
            if (-EPSILON < det && det < EPSILON)
                return false;

            distance = Vector3.Dot(n, c - ray.origin) / det;
            return true;
        }
		public virtual Vector3 ProjectOn(Vector3 worldPos, float distance = 0f) {
			var layerPos = LayerToWorld.InverseTransformPoint(worldPos);
			layerPos.z = distance;
			return LayerToWorld.TransformPoint(layerPos);
		}
		#endregion

		#region public
		public Events GetEvents() { return events; }
		#endregion

		#region private
		protected virtual void NotifyLayerOnChange() {
            foreach (var c in transform.Children<ILayerListener>(false))
                c.TargetOnChange(this);
			if (events != null)
				events.LayerOnChange.Invoke(this);
		}
        protected virtual void GenerateLayerData() {
            var localScale = transform.localScale;
            localScale.z = 1f;

            var layer = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            var local = Matrix4x4.Scale(localScale);
            LayerToWorld.Reset(layer);
            LocalToLayer.Reset(local);
            LocalToWorld.Reset(layer, local);
        }
		#endregion

		#region classes
		public interface ILayerListener : IChangeListener<Layer> {}
		[System.Serializable]
		public class LayerEvent : UnityEngine.Events.UnityEvent<Layer> { }
		[System.Serializable]
		public class Events {
			public LayerEvent LayerOnChange;
		}
		#endregion
	}
}
