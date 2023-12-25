using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public class RegionLineRenderersSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;
        [SerializeField] private GameObject _prefab;

        [Button("Spawn")]
        public void CreateLineRenderers()
        {
            for (int i = 0; i < _objects.Length - 1; i++)
            {
                // Создаем новый игровой объект для линии
                GameObject lineObj = Instantiate(_prefab, transform);
                _prefab.name = "Line" + i;

                // Добавляем компонент LineRenderer к новому объекту
                LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

                // Устанавливаем количество точек линии (в данном примере - 2 точки)
                lineRenderer.positionCount = 2;

                // Устанавливаем начальную и конечную точки линии в позиции объектов
                lineRenderer.SetPosition(0, _objects[i].transform.position);
                lineRenderer.SetPosition(1, _objects[i + 1].transform.position);
            }
        }
    }
}