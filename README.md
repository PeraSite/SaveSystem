# Save System

Provides an powerful save system for Unity.

- **Flexible** : Everything is interface and injected from Zenject.<br>You can write your own Storage, Data Serializer implementation.
- **Easy to use** : You can save and load data from `MonoBehaviour` with just make class extending `Saver`.
- **Fast** : All data is cached to memory in tree-like structure.
- **Supports Editor** : You can save and load data from Editor. _(Only if project installed Odin Inspector)_
- **Scope-based** : You can implement `IScope` for managing complex data structures like Save Slot.

## Table of Contents
<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [Dependencies](#dependencies)
  - [Optional](#optional)
- [Installation](#installation)
  - [Using UPM(Unity Package Manager)](#using-upmunity-package-manager)
  - [Import as source](#import-as-source)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## Dependencies

> Make sure you installed those dependencies!

- [Zenject](https://github.com/modesttree/Zenject) (Recommend to
  use [Extenject](https://github.com/Mathijs-Bakker/Extenject) instead)
- [UniTask](https://github.com/Cysharp/UniTask)

### Optional

- [Odin Inspector](https://odininspector.com/)<br>
- If you have Odin Inspector installed, you can use
  - OdinDataSerializer(uses Odin Serializer)
  - SerializedSaver(based on SerializedMonoBehaviour)
  - SaveEditor(Provide direct access to SaveSystem)

## Installation

### Using UPM(Unity Package Manager)

This is most recommended way to install the Save System.

![image](https://user-images.githubusercontent.com/19837403/211511847-c5ae8573-1a29-4238-b376-2744ce3220bc.png)
![image](https://user-images.githubusercontent.com/19837403/211512349-1901b308-19f9-4fa5-b1fa-bf40455e07ab.png)

Add git url `https://github.com/PeraSite/SaveSystem.git?path=Assets/SaveSystem` to projects.

### Import as source

1. Clone this repository
2. Copy `Assets/SaveSystem` folder from repository, to your project
