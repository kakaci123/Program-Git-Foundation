/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package bst_practice;

/**
 *
 * @author ChenWei
 */
public abstract class BinaryTree<T> {

    protected T value;

    public T getValue() {
        return value;
    }

    public void setValue(T value) {
        this.value = value;
    }

    public abstract int size();

    public abstract int depth();

    public abstract BinaryTree<T> parent();

    public abstract BinaryTree<T> left();

    public abstract BinaryTree<T> right();

    public abstract int level();

    public BinaryTree(T value) {
        this.value = value;
    }

    public abstract void addLeft(T value) throws Exception;

    public abstract void addRight(T value) throws Exception;

    public abstract void addLeft(BinaryTree<T> tree) throws Exception;

    public abstract void addRight(BinaryTree<T> tree) throws Exception;

    public abstract void remove();

    public abstract BinaryTree<T> copy() throws Exception;

    public static <T> void copy(BinaryTree<T> srcTree, BinaryTree<T> destTree) throws Exception {
        if (srcTree.left() != null) {
            destTree.addLeft(srcTree.left().getValue());
            copy(srcTree.left(), destTree.left());
        }
        if (srcTree.right() != null) {
            destTree.addRight(srcTree.right().getValue());
            copy(srcTree.right(), destTree.right());
        }
    }
}
