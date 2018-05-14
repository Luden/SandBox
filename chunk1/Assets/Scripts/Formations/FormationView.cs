using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Formations
{
    public class FormationView : MonoBehaviour
    {
        public List<Unit> Units;

        private Formation _formation;
        private Vector3 _oldPos;

        public void OnDrawGizmosSelected()
        {
            if (Units.Count == 0)
                return;

            _oldPos = transform.position;
            _formation = new Formation();
            _formation.Init(Units.Select(x => x.transform.position), _oldPos);
            _formation.Build();

            Gizmos.color = Color.green;
            for (int i = 0; i < Units.Count; i++)
                Gizmos.DrawSphere(_formation.GetTargePos(i) + new Vector3(0, 0.5f, 0), 0.5f);
        }
    }
}
