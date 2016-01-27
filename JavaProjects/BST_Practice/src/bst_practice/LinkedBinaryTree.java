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
public class LinkedBinaryTree<T> extends BinaryTree<T> {

    protected int size;

    @Override
    public int size() {
        // TODO Auto-generated method stub
        return size;
    }

    protected int depth;

    @Override
    public int depth() {
        // TODO Auto-generated method stub
        return depth;
    }

    protected LinkedBinaryTree<T> parent;

    @Override
    public BinaryTree<T> parent() {
        // TODO Auto-generated method stub
        return parent;
    }

    protected LinkedBinaryTree<T> left;

    @Override
    public BinaryTree<T> left() {
        // TODO Auto-generated method stub
        return left;
    }

    protected LinkedBinaryTree<T> right;

    @Override
    public BinaryTree<T> right() {
        // TODO Auto-generated method stub
        return right;
    }

    protected int level;

    @Override
    public int level() {
        // TODO Auto-generated method stub
        return level;
    }

    public LinkedBinaryTree(T value) {
        super(value);
        size = 1;
        depth = 1;
        level = 1;
    }

    @Override
    public void addLeft(T value) throws Exception {
        add(true, value);
    }

    @Override
    public void addRight(T value) throws Exception {
        add(false, value);
    }

    @Override
    public void addLeft(BinaryTree<T> tree) throws Exception {
        add(true, tree);
    }

    @Override
    public void addRight(BinaryTree<T> tree) throws Exception {
        add(false, tree);
    }

    protected void add(boolean isLeft, T value) throws Exception {
        add(isLeft, new LinkedBinaryTree<T>(value));
    }

    protected void add(boolean isLeft, BinaryTree<T> tree) throws Exception {
        LinkedBinaryTree<T> tmpTree = null;
        if (tree instanceof LinkedBinaryTree) {
            tmpTree = (LinkedBinaryTree<T>) tree;
        } else {
            tmpTree = new LinkedBinaryTree<T>(tree.getValue());
            BinaryTree.copy(tree, tmpTree);
        }
        if (isLeft) {
            if (left != null) {
                throw new Exception("Child is existed");
            }
            left = tmpTree;
            left.level = level + 1;
            left.parent = this;
        } else {
            if (right != null) {
                throw new Exception("Child is existed");
            }
            right = tmpTree;
            right.level = level + 1;
            right.parent = this;

        }
        if (depth == 1) {
            depth = 2;
            bubbleDepth();
        }
        ++size;
        bubbleCount(1);
    }

    @Override
    public void remove() {
        // TODO Auto-generated method stub
        LinkedBinaryTree<T> sibling;
        if (parent == null) {
            return;
        } else if (parent.left == this) {
            parent.left = null;
            sibling = parent.right;
        } else if (parent.right == this) {
            parent.right = null;
            sibling = parent.left;
        } else {
            return;
        }

        if (depth + 1 == parent.depth) {
            if (sibling == null || sibling.depth < depth) {
                parent.updateDepth();
            }
        }
        parent.size -= size;
        parent.bubbleCount(-size);
        parent = null;
    }

    @Override
    public BinaryTree<T> copy() throws Exception {
        // TODO Auto-generated method stub
        LinkedBinaryTree<T> cloneTree = new LinkedBinaryTree<T>(this.getValue());
        if (this.parent == null) {
            cloneTree.size = this.size;
            cloneTree.depth = this.depth;
            cloneTree.level = this.level;
            cloneTree.left = (LinkedBinaryTree<T>) this.left.copy();
            cloneTree.right = (LinkedBinaryTree<T>) this.right.copy();
            cloneTree.left.parent = cloneTree;
            cloneTree.right.parent = cloneTree;
        } else {
            BinaryTree.copy(this, cloneTree);
        }
        return cloneTree;
    }

    protected void bubbleDepth() {
        if (parent == null) {
            return;
        }

        if (depth + 1 > parent.depth) {
            parent.depth = depth + 1;
            parent.bubbleDepth();
        }
    }

    protected void updateDepth() {
        int tmpDepth = depth;
        depth = 1;

        if (left != null) {
            depth = left.depth + 1;
        }
        if (right != null && right.depth + 1 > depth) {
            depth = right.depth + 1;
        }

        if (tmpDepth == depth || parent == null) {
            return;
        }
        if (tmpDepth + 1 == parent.depth) {
            parent.updateDepth();
        }
    }

    protected void bubbleCount(int diff) {
        if (parent == null) {
            return;
        }

        parent.size += diff;
        parent.bubbleCount(diff);
    }
}
