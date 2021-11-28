# GameKit说明

### Core

- Features
  - TopDownmove/TopDownmoveGrid 脚本，直接挂上即可
- Manager（大部分是单例）
  - AudioManager，Unity原生Audio组件的Manager，大概用不到
  - BaseManager，所有Manager的父类
  - EventCenter，估计使用量最大。事件中心管理器，组件间通信可以借助AddListener和EventTrigger两个方法解耦，可指定泛型传递参数，如果是值类型参数可以利用KeyValuePair传递两个参数。注意：EventCenter是广播系统。技巧：事件名称可以用Enum+tostring()的方法命名，好处是可以利用IDE对Enum查找Reference，能够快速定位谁在监听谁在触发。
  - GameManager，可以直接挂在场景中也可以座位Mono虚拟机里所有单例Manager的父类，应该用得着。
  - InputManager，把Input的方法封装了一边方便控制输入的开启预关闭，也可以利用EventCenter广播输入命令。应该用得着，可以采用新版InputSystem替代。
  - JsonManager，配合StreamingAsset的Json序列化器，只是根据加载对象是path和file，加载结果是List/Dic做了简单封装，大概用不着。
  - MonoController+MonoManager，MonoManager可以让没有继承MonoBehaviour的类使用Mono里的方法，统一通过MonoController传递。大概用得着，对于一些静态类、非Mono单例、Procedure需求有帮助。
  - MultiSceneMono，用不着，仅用于多场景工作流，也还没写，可以删了。
  - PoolManager，缓冲池，和ResourceManager以耦合，当时写有点问题，需要使用时我再改。
  - ResourceManager，把Resources封装了一边，封装了加载GameObject直接实例化，和加载Path的功能
  - ScenesManager，封装了Unity自带的SceneManager，Asyn抛出AsynOperation方便制作加载条。
  - 总结一下：大概率用得到EventCenter和MonoManager，其他小概率用得到。
- StateMachine
  - 使用可以看StateA/StateB和StateTrigger
- MultiScene
  - 仅用于多场景工作流
- UI
  - FaryUI的话用不着

# Editor

- Excel用不着
- Common里只有一个分离Sprite的





