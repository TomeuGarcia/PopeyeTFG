using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChainSpawner : MonoBehaviour
{
  [SerializeField] private GameObject _link;
  [SerializeField] private Rigidbody from;
  [SerializeField] private Rigidbody to;
  [SerializeField] private int _length = 20;
  [SerializeField] private float _linkLength = 1f;
  
  private Rigidbody[] _links;

  void Awake()
  {
    _links = new Rigidbody[_length];
    var prev = Instantiate(_link, transform.position, Quaternion.identity, transform);
    _links[0] = prev.GetComponent<Rigidbody>();

    var joint = to.GetComponent<Joint>();
    joint.connectedBody = _links[0];
    var anchor = joint.connectedAnchor;
    anchor.y = -_linkLength;
    joint.connectedAnchor = anchor;

    for (var i = 1; i < _length; i++)
    {
      var link = Instantiate(_link, transform.position, Quaternion.identity, transform);
      joint = prev.GetComponent<Joint>();
      _links[i] = link.GetComponent<Rigidbody>();
      LinkJoint(_links[i], joint, _linkLength);

      if (i % 2 == 0)
      {
        joint.transform.GetChild(0).Rotate(0, 90, 0);

        if (i < _length - 1)
        {
          //ConfigurableJoint extraJoint = joint.gameObject.AddComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
          //LinkJoint(_links[i + 1], extraJoint, _linkLength*2);
        }
      }

      prev = link;
    }

    Destroy(prev.GetComponent<Joint>());
    joint = from.GetComponent<Joint>();
    joint.connectedBody = prev.GetComponent<Rigidbody>();
    anchor = joint.connectedAnchor;
    anchor.y = _linkLength;
    joint.connectedAnchor = anchor;

  }

  private void LinkJoint(Rigidbody current_Rigidbody, Joint previous_Joint, float linkLength)
  {
    Joint joint = previous_Joint;
    
    joint.connectedBody = current_Rigidbody;
    Vector3 anchor = joint.connectedAnchor;
    anchor.y = -linkLength;
    joint.connectedAnchor = anchor;
    anchor = joint.anchor;
    anchor.y = linkLength;
    joint.anchor = anchor;

  }


}
